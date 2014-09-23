using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using PodcastReader.Infrastructure.Http;

namespace PodcastReader.Phone8.Infrastructure.Http
{
    public class AwaitableTransferRequest : IAwaitableTransfer
    {
        private readonly BackgroundTransferRequest _request;
        private readonly TaskCompletionSource<object> _tcs;

        public AwaitableTransferRequest(BackgroundTransferRequest request)
        {
            _request = request;
            _tcs = new TaskCompletionSource<object>();

            var transferChanged = Observable.FromEventPattern<BackgroundTransferEventArgs>(
                    h => request.TransferStatusChanged += h,
                    h => request.TransferStatusChanged -= h)
                .Select(p => p.EventArgs.Request)
                .StartWith(request)
                .Distinct(r => r.TransferStatus);

            transferChanged.Subscribe(req =>
                {
                    if (req.TransferStatus == TransferStatus.Completed)
                    {
                        if (req.TransferError != null)
                            _tcs.SetException(req.TransferError);
                        else
                            _tcs.SetResult(null);
                    }
                    else if (req.TransferStatus == TransferStatus.Unknown)
                    {
                        _tcs.SetException(new TransferStatusUnknownException());
                    }
                });
        }

        public Task TransferTask { get { return _tcs.Task; } }
        public Uri UploadLocation { get { return _request.UploadLocation; } }
        public Uri DownloadLocation { get { return _request.DownloadLocation; } }

        public BackgroundTransferRequest UnderlyingRequest { get { return _request; } }
    }
}