using System;
using System.Threading;
using System.Threading.Tasks;

namespace PodcastReader.Phone8.Infrastructure.Http
{
    public interface IBackgroundDownloader
    {
        Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation);
    }

    public struct ProgressValue
    {
        private readonly long _current;
        private readonly long _total;

        public ProgressValue(long current, long total)
        {
            _current = current;
            _total = total;
        }

        public long Current { get { return _current; } }
        public long Total { get { return _total; } }
    }

}