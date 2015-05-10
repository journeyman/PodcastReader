using System;
using System.Reactive.Linq;
using JetBrains.Annotations;
using PodcastReader.Infrastructure.Http;
using ReactiveUI;

namespace PodcastReader.Infrastructure.Caching
{
    public class CachingStateVm : ReactiveObject, ICachingState
    {
        [NotNull] private readonly CachingState _cachingState;
        [NotNull] private readonly ObservableAsPropertyHelper<bool> _isFullyCached;
        [NotNull] private readonly ObservableAsPropertyHelper<ulong> _finalSize;
        [NotNull] private readonly ObservableAsPropertyHelper<ulong> _cachedSize;
        [NotNull] private readonly ObservableAsPropertyHelper<bool> _isInitialized; 

        public CachingStateVm(CachingState cachingState)
        {
            _cachingState = cachingState;
            var progress = cachingState.Progress;
            _finalSize = progress.Select(x => x.Total).ToProperty(this, x => x.FinalSize);
            _cachedSize = progress.Select(x => x.Current).ToProperty(this, x => x.CachedSize);
            _isFullyCached = progress.Select(x => x.Current != 0UL && x.Current == x.Total).ToProperty(this, x => x.IsFullyCached, false);
            _isInitialized = this.WhenAny(x => x.FinalSize, ch => ch.Value > 0).ToProperty(this, x => x.IsInitialized, false);
        }

        public ulong CachedSize => _cachedSize.Value;
        public ulong FinalSize => _finalSize.Value;
        public bool IsFullyCached => _isFullyCached.Value;
        public bool IsInitialized => _isInitialized.Value;
        public Uri CachedUri => _cachingState.CachedUri;
    }
}