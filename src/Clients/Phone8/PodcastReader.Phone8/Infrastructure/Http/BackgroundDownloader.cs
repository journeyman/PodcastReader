using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;
using PodcastReader.Infrastructure.Utils;

namespace PodcastReader.Phone8.Infrastructure.Http
{
    
    public class BackgroundDownloader : IBackgroundDownloader
    {
        private readonly IBackgroundTransferStorage _storage;
        private readonly IBackgroundTransferConfig _config;
        private readonly IEqualityComparer<Uri> _urlEquator;
        private IImmutableDictionary<Uri, IAwaitableTransfer> _requests = ImmutableDictionary<Uri, IAwaitableTransfer>.Empty;

        public BackgroundDownloader(IBackgroundTransferStorage storage, IBackgroundTransferConfig config)
        {
            _storage = storage;
            _config = config;

            _urlEquator = new FuncEqualityComparer<Uri>((url1, url2) => url1 == url2, url => url.GetHashCode());
        }

        public IImmutableDictionary<Uri, IAwaitableTransfer> ActiveRequests { get { return _requests; } } 

        /// <summary>
        /// Completely Sync!!
        /// </summary>
        public async Task<IImmutableDictionary<Uri, IAwaitableTransfer>> Update()
        {
            _requests = BackgroundTransferService.Requests
                .Select(r => new {Transfer = (IAwaitableTransfer)new AwaitableTransferRequest(r), RequestUri = r.RequestUri})
                .ToImmutableDictionary(x => x.RequestUri, x => x.Transfer,
                                       _urlEquator);
            return _requests;
        }

        public async Task ForgetAbout(Uri transferUri)
        {
            BackgroundTransferRequest request;
            IAwaitableTransfer awaitableRequest;
            if (_requests.TryGetValue(transferUri, out awaitableRequest))
            {
                var actual = (AwaitableTransferRequest) awaitableRequest;
                request = actual.UnderlyingRequest;
            }
            else
            {
                request = BackgroundTransferService.Requests.SingleOrDefault(r => _urlEquator.Equals(r.RequestUri, transferUri));
            }
            BackgroundTransferService.Remove(request);
            await _storage.RemoveFile(request.DownloadLocation);
        }

        /// <summary>
        /// A bit synchronous: calls BackgroundTransferService.Requests on calling thread
        /// </summary>
        public async Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            var uri = new Uri(url);
            IAwaitableTransfer awaitableTransfer = null;
            if (!_requests.TryGetValue(uri, out awaitableTransfer))
            {
                var request = BackgroundTransferService.Requests.FirstOrDefault(r => _urlEquator.Equals(r.RequestUri, uri));
                if (request == null)
                {
                    var resourceUri = new Uri(url);
                    request = new BackgroundTransferRequest(resourceUri, new Uri(_storage.GetTransferUrl(resourceUri.LocalPath)));
                    request.TransferPreferences = _config.Preferences.ToNative();
                    BackgroundTransferService.Add(request);
                }
                awaitableTransfer = new AwaitableTransferRequest(request);
                _requests = _requests.Add(uri, awaitableTransfer);
            }

            return await awaitableTransfer.ToDownload();
        }
    }
}