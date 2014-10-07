using ReactiveUI;

namespace PodcastReader.Infrastructure.Caching
{
    public interface ICachingState// : IReactiveObject
    {
        ulong? CachedSize { get; }
        ulong? FinalSize { get; }
        bool IsFullyCached { get; }
        IReactiveProgress<ulong> Progress { get; }
    }
}