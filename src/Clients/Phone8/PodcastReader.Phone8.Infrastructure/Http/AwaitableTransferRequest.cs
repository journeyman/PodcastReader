using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;

namespace PodcastReader.Infrastructure.Http
{
    public class AwaitableTransferRequest
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
                            _tcs.SetException((Exception) req.TransferError);
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

        public BackgroundTransferRequest UnderlyingRequest { get { return _request; } }
    }
}