using System;
using ReactiveUI;

namespace Pr.Core.Caching
{
    public interface ICachingState : IReactiveNotifyPropertyChanged<IReactiveObject>, IHandleObservableErrors
    {
        ulong CachedSize { get; }
        ulong FinalSize { get; }
        bool IsFullyCached { get; }
        bool IsInitialized { get; }
        Uri CachedUri { get; }
    }
}