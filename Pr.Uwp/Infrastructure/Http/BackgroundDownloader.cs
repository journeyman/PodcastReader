using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Pr.Core.Http;
using Pr.Core.Storage;
using Pr.Core.Utils;

namespace Pr.Phone8.Infrastructure.Http
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

	    public IImmutableDictionary<Uri, IAwaitableTransfer> ActiveRequests => _requests;

	    public IImmutableDictionary<Uri, IAwaitableTransfer> Update()
        {
            //_requests = BackgroundTransferService.Requests
            //    //TODO: refactor to be able to assign Progress reporting to ongoing AwaitableTransferRequests
            //    .Select(r => new {Transfer = (IAwaitableTransfer)new AwaitableTransferRequest(r, null), RequestUri = r.RequestUri})
            //    .ToImmutableDictionary(x => x.RequestUri, x => x.Transfer, _urlEquator);
            //return _requests;
		    return null;
        }

        public async Task ForgetAbout(Uri transferUri)
        {
			//TODO: optimize retrieving request from cache (for now remote uri is used as key, not a transfer)
			//if (_requests.TryGetValue(transferUri, out awaitableRequest))
			//{
			//    var actual = (AwaitableTransferRequest) awaitableRequest;
			//    request = actual.UnderlyingRequest;
			//}
			//else
			//{
			//    request = BackgroundTransferService.Requests.SingleOrDefault(r => _urlEquator.Equals(r.DownloadLocation, transferUri));
			//}
			
			//request = BackgroundTransferService.Requests.SingleOrDefault(r => _urlEquator.Equals(r.DownloadLocation, transferUri));
			//BackgroundTransferService.Remove(request);
			//await _storage.RemoveFile(request.DownloadLocation);
		}

		/// <summary>
		/// A bit synchronous: calls BackgroundTransferService.Requests on calling thread
		/// </summary>
		public IAwaitableTransfer Load(Uri uri, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            //IAwaitableTransfer awaitableTransfer = null;
            //if (!_requests.TryGetValue(uri, out awaitableTransfer))
            //{
            //    var request = BackgroundTransferService.Requests.FirstOrDefault(r => _urlEquator.Equals(r.RequestUri, uri));
            //    if (request == null)
            //    {
            //        request = new BackgroundTransferRequest(uri, new Uri(_storage.GetTransferUrl(uri.LocalPath), UriKind.Relative));
            //        request.TransferPreferences = _config.Preferences.ToNative();
            //        BackgroundTransferService.Add(request);
            //    }
            //    awaitableTransfer = new AwaitableTransferRequest(request, progress);
            //    _requests = _requests.Add(uri, awaitableTransfer);
            //}

            //return awaitableTransfer;

			return null;
        }
    }
}
