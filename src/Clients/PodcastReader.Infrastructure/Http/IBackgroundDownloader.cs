using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Http
{
    public interface IBackgroundDownloader
    {
        Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation);
        IImmutableDictionary<Uri, IAwaitableTransfer> ActiveRequests { get; }
        Task ForgetAbout(Uri transferUri);
    }
}