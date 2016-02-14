using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;
using Pr.Core.Http;
using Splat;

namespace Pr.Phone8.Infrastructure.Http
{
    public class AwaitableTransferRequest : IAwaitableTransfer, IEnableLogger
    {
        private readonly BackgroundTransferRequest _request;
        private readonly TaskCompletionSource<object> _tcs;

        public AwaitableTransferRequest(BackgroundTransferRequest request, IProgress<ProgressValue> progress)
        {
            _request = request;
            _tcs = new TaskCompletionSource<object>();

            Observable.FromEventPattern<BackgroundTransferEventArgs>(
                    h => request.TransferProgressChanged += h,
                    h => request.TransferProgressChanged -= h)
                .Select(args => args.EventArgs.Request)
                .Subscribe(r =>
                    {
                        //TODO: handle uploading BytesSent (should be handled on higher level in Wrapper.ToUpload())
                        var progressValue = new ProgressValue((ulong) r.BytesReceived, (ulong) r.TotalBytesToReceive);
                        this.Log().Debug("{0}: current {1}, total {2}", Path.GetFileName(r.RequestUri.OriginalString), progressValue.Current, progressValue.Total);
                        progress?.Report(progressValue);
                    });

            Observable.FromEventPattern<BackgroundTransferEventArgs>(
                    h => request.TransferStatusChanged += h,
                    h => request.TransferStatusChanged -= h)
                .Select(p => p.EventArgs.Request)
                .StartWith(request)
                .Distinct(r => r.TransferStatus)
                .Subscribe(req =>
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