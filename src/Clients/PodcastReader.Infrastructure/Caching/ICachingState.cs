using ReactiveUI;

namespace PodcastReader.Infrastructure.Caching
{
    public interface ICachingState : IReactiveNotifyPropertyChanged<IReactiveObject>, IHandleObservableErrors
    {
        ulong CachedSize { get; }
        ulong FinalSize { get; }
        bool IsFullyCached { get; }
    }
}