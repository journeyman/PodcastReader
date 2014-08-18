using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using PodcastReader.Phone8.Infrastructure.Storage;

namespace PodcastReader.Phone8.Infrastructure.Http
{
    
    public class BackgroundDownloader : IBackgroundDownloader
    {
        private readonly IBackgroundTransferStorage _storage;
        private readonly Func<string, string, bool> _urlEquator; 


        public BackgroundDownloader(IBackgroundTransferStorage storage)
        {
            _storage = storage;

            _urlEquator = (url1, url2) => url1 == url2;
        }

        /// <summary>
        /// A bit synchronous: calls BackgroundTransferService.Requests on calling thread
        /// </summary>
        public Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            var request = BackgroundTransferService.Requests.FirstOrDefault(r => _urlEquator(r.RequestUri.AbsoluteUri, url));
            if (request == null)
            {
                var resourceUri = new Uri(url);
                request = new BackgroundTransferRequest(resourceUri, new Uri(_storage.GetTransferUrl(resourceUri.LocalPath)));
                BackgroundTransferService.Add(request);
            }
            
            return request.DownloadLocation;
        }
    }
}
