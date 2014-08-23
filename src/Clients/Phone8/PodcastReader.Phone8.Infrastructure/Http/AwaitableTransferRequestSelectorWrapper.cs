using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Http
{
    public static class Exts
    {
        public static AwaitableTransferRequestSelectorWrapper<Uri> ToDownload(
            this AwaitableTransferRequest awaitableRequest)
        {
            return new AwaitableTransferRequestSelectorWrapper<Uri>(awaitableRequest, r => r.UnderlyingRequest.DownloadLocation);
        }
        
        public static AwaitableTransferRequestSelectorWrapper<Uri> ToUpload(
            this AwaitableTransferRequest awaitableRequest)
        {
            return new AwaitableTransferRequestSelectorWrapper<Uri>(awaitableRequest, r => r.UnderlyingRequest.UploadLocation);
        }

        public struct AwaitableTransferRequestSelectorWrapper<T>
        {
            private readonly AwaitableTransferRequest _awaitableTransfer;
            private readonly Func<AwaitableTransferRequest, T> _resultSelector;

            public AwaitableTransferRequestSelectorWrapper(AwaitableTransferRequest awaitableTransfer, Func<AwaitableTransferRequest, T> resultSelector)
            {
                _awaitableTransfer = awaitableTransfer;
                _resultSelector = resultSelector;
            }

            public TaskAwaiter<T> GetAwaiter()
            {
                var localCopy = this;
                return _awaitableTransfer
                    .TransferTask
                    .ContinueWith(async t =>
                    {
                        await t;
                        return localCopy._resultSelector(localCopy._awaitableTransfer);
                    })
                    .Unwrap()
                    .GetAwaiter();
            }
        }

    }
}