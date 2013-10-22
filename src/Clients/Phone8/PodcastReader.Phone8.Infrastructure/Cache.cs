using Akavache;
using ReactiveUI;

namespace PodcastReader.Infrastructure
{
    public static class Cache
    {
        private static readonly IBlobCache _local = RxApp.DependencyResolver.GetService<IBlobCache>();
        public static IBlobCache Local {get { return _local; }}
    }
}
