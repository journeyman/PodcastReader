using Akavache;
using Splat;

namespace PodcastReader.Infrastructure
{
    public static class Cache
    {
        private static readonly IBlobCache _local = Locator.Current.GetService<IBlobCache>();
        public static IBlobCache Local {get { return _local; }}
    }
}
