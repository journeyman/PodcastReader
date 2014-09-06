using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using PodcastReader.Infrastructure.Storage;
using PodcastReader.Infrastructure.Utils;

namespace PodcastReader.Infrastructure.Http
{
    
    public class BackgroundDownloader : IBackgroundDownloader
    {
        private readonly IBackgroundTransferStorage _storage;
        private readonly IBackgroundTransferConfig _config;
        private readonly IEqualityComparer<Uri> _urlEquator;
        private IImmutableDictionary<Uri, AwaitableTransferRequest> _requests = ImmutableDictionary<Uri, AwaitableTransferRequest>.Empty;

        public BackgroundDownloader(IBackgroundTransferStorage storage, IBackgroundTransferConfig config)
        {
            _storage = storage;
            _config = config;

            _urlEquator = new FuncEqualityComparer<Uri>((url1, url2) => url1 == url2, url => url.GetHashCode());
        }

        public IImmutableDictionary<Uri, AwaitableTransferRequest> ActiveRequests { get { return _requests; } } 

        /// <summary>
        /// Completely Sync!!
        /// </summary>
        public async Task<IImmutableDictionary<Uri, AwaitableTransferRequest>> Update()
        {
            _requests = BackgroundTransferService.Requests
                .Select(r => new AwaitableTransferRequest(r))
                .ToImmutableDictionary(r => r.UnderlyingRequest.RequestUri,
                                       _urlEquator);
            return _requests;
        }

        public void ForgetAbout(Uri transferUri)
        {
            BackgroundTransferRequest request;
            AwaitableTransferRequest awaitableRequest;
            if (_requests.TryGetValue(transferUri, out awaitableRequest))
            {
                request = awaitableRequest.UnderlyingRequest;
            }
            else
            {
                request = BackgroundTransferService.Requests.SingleOrDefault(r => _urlEquator.Equals(r.RequestUri, transferUri));
            }
            BackgroundTransferService.Remove(request);
        }

        /// <summary>
        /// A bit synchronous: calls BackgroundTransferService.Requests on calling thread
        /// </summary>
        public async Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            var uri = new Uri(url);
            AwaitableTransferRequest awaitableTransfer = null;
            if (!_requests.TryGetValue(uri, out awaitableTransfer))
            {
                var request = BackgroundTransferService.Requests.FirstOrDefault(r => _urlEquator.Equals(r.RequestUri, uri));
                if (request == null)
                {
                    var resourceUri = new Uri(url);
                    request = new BackgroundTransferRequest(resourceUri, new Uri(_storage.GetTransferUrl(resourceUri.LocalPath)));
                    request.TransferPreferences = _config.Preferences;
                    BackgroundTransferService.Add(request);
                }
                awaitableTransfer = new AwaitableTransferRequest(request);
                _requests = _requests.Add(uri, awaitableTransfer);
            }

            return await awaitableTransfer.ToDownload();
        }
    }
}
