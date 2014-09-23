using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Http
{
    public static class Exts
    {
        public static AwaitableTransferRequestSelectorWrapper<Uri> ToDownload(
            this IAwaitableTransfer awaitableRequest)
        {
            return new AwaitableTransferRequestSelectorWrapper<Uri>(awaitableRequest, r => r.DownloadLocation);
        }
        
        public static AwaitableTransferRequestSelectorWrapper<Uri> ToUpload(
            this IAwaitableTransfer awaitableRequest)
        {
            return new AwaitableTransferRequestSelectorWrapper<Uri>(awaitableRequest, r => r.UploadLocation);
        }

        public struct AwaitableTransferRequestSelectorWrapper<T>
        {
            private readonly IAwaitableTransfer _awaitableTransfer;
            private readonly Func<IAwaitableTransfer, T> _resultSelector;

            public AwaitableTransferRequestSelectorWrapper(IAwaitableTransfer awaitableTransfer, Func<IAwaitableTransfer, T> resultSelector)
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