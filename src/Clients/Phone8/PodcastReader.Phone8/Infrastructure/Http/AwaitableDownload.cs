using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;

namespace PodcastReader.Phone8.Infrastructure.Http
{
    public class TransferStatusUnknownException : Exception { }
    public struct AwaitableDownload
    {
        private readonly BackgroundTransferRequest _request;
        private TaskCompletionSource<object> _tcs;

        public AwaitableDownload(BackgroundTransferRequest request)
        {
            _request = request;
            _tcs = new TaskCompletionSource<object>();

            var transferChanged = Observable.FromEventPattern<BackgroundTransferEventArgs>(
                h => request.TransferStatusChanged += h,
                h => request.TransferStatusChanged -= h)
                .Select(p => p.EventArgs.Request.TransferStatus);

            transferChanged.Subscribe(status =>
            {
                if (status == TransferStatus.Completed)
                {
                    
                }
                else if (status == TransferStatus.Unknown)
                {
                    _tcs.SetException(new TransferStatusUnknownException());
                }
            });

            _transferChangedSub = 
                .Su;

            _request.Tran+=_request_TransferStatusChanged;
        }
    }
}