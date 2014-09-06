using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Http
{
    public interface IBackgroundDownloader
    {
        Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation);
        IImmutableDictionary<Uri, AwaitableTransferRequest> ActiveRequests { get; }
        Task<IImmutableDictionary<Uri, AwaitableTransferRequest>> Update();
        Task ForgetAbout(Uri transferUri);
    }
}