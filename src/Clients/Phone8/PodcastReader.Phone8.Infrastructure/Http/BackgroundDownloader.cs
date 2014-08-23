using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using PodcastReader.Infrastructure.Storage;

namespace PodcastReader.Infrastructure.Http
{
    
    public class BackgroundDownloader : IBackgroundDownloader
    {
        private readonly IBackgroundTransferStorage _storage;
        private readonly IBackgroundTransferConfig _config;
        private readonly Func<string, string, bool> _urlEquator;
        private IImmutableList<BackgroundTransferRequest> _requests = ImmutableList<BackgroundTransferRequest>.Empty;

        public BackgroundDownloader(IBackgroundTransferStorage storage, IBackgroundTransferConfig config)
        {
            _storage = storage;
            _config = config;

            _urlEquator = (url1, url2) => url1 == url2;
        }

        /// <summary>
        /// A bit synchronous: calls BackgroundTransferService.Requests on calling thread
        /// </summary>
        public async Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            var request = BackgroundTransferService.Requests.FirstOrDefault(r => _urlEquator(r.RequestUri.AbsoluteUri, url));
            if (request == null)
            {
                var resourceUri = new Uri(url);
                request = new BackgroundTransferRequest(resourceUri, new Uri(_storage.GetTransferUrl(resourceUri.LocalPath)));
                request.TransferPreferences = _config.Preferences;
                BackgroundTransferService.Add(request);
            }

            return await new AwaitableTransferRequest(request).ToDownload();
        }
    }
}
