using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Akavache;
using JetBrains.Annotations;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;
using ReactiveUI;

namespace PodcastReader.Infrastructure.Caching
{
    public class CachingState : ReactiveObject, ICachingState
    {
        [NotNull] private readonly IPodcastItem _item;
        [NotNull] private readonly IBackgroundDownloader _downloader;
        [NotNull] private readonly IPodcastsStorage _storage;
        [NotNull] private readonly ObservableAsPropertyHelper<bool> _isFullyCached;
        [NotNull] private readonly ObservableAsPropertyHelper<ulong?> _finalSize;
        [NotNull] private readonly ObservableAsPropertyHelper<ulong?> _cachedSize;
        [NotNull] private readonly ObservableAsPropertyHelper<IReactiveProgress<ulong>> _progress;
        [NotNull] private readonly ISubject<IReactiveProgress<ulong>> _progressSubj = new Subject<IReactiveProgress<ulong>>();


        public CachingState(IPodcastItem item, IBackgroundDownloader downloader, IPodcastsStorage storage)
        {
            _item = item;
            _downloader = downloader;
            _storage = storage;

            _progress = _progressSubj.ToProperty(this, x => x.Progress);
            _finalSize = this.WhenAny(x => x.Progress, change => change.Value.FinalState).Select(x => (ulong?)x).ToProperty(this, x => x.FinalSize, null);
            _isFullyCached = this.WhenAnyObservable(x => x.Progress).Select(x => x == this.Progress.FinalState).ToProperty(this, x => x.IsFullyCached, false);
            _cachedSize = this.WhenAnyObservable(x => x.Progress).Select(x => (ulong?)x).ToProperty(this, x => x.CachedSize);
        }

        [CanBeNull]
        public IReactiveProgress<ulong> Progress { get { return _progress.Value; } }
        [CanBeNull]
        public ulong? CachedSize { get { return _cachedSize.Value; } }
        [CanBeNull]
        public ulong? FinalSize { get { return _finalSize.Value; } }
        public bool IsFullyCached { get { return _isFullyCached.Value; } }

        public void Init()
        {
            Observable.Create<IReactiveProgress<ulong>>(async observer =>
                {
                    IObservable<IReactiveProgress<ulong>> observable = null;

                    var cacheInfo = await Cache.Local.GetObject<PodcastCacheInfo>(_item.PodcastUri.AbsoluteUri)
                        .Catch(Observable.Return<PodcastCacheInfo>(null));

                    if (cacheInfo == null || cacheInfo.Downloaded < cacheInfo.FinalSize)
                    {
                        //if (not downloaded OR download not finished)
                        var progress = new OngoingReactiveProgress();
                        var transferUri =
                            await _downloader.Load(_item.PodcastUri.AbsoluteUri, progress, CancellationToken.None);
                        //TODO: think how to implement via Move (should atomically call Move and Save info into cache)
                        var realUri = await _storage.CopyFromTransferTempStorage(transferUri, _item);
                        var newCacheInfo = new PodcastCacheInfo()
                        {
                            FileUri = realUri,
                            FinalSize = progress.FinalState,
                            Downloaded = progress.FinalState
                        };
                        await Cache.Local.InsertObject(_item.PodcastUri.AbsoluteUri, newCacheInfo);
                        await _downloader.ForgetAbout(transferUri);
                        observable = Observable.Return(progress);
                    }
                    else
                    {
                        observable = Observable.Return(new FinishedReactiveProgress<ulong>(cacheInfo.FinalSize));
                    }

                    return observable.Subscribe(observer);
                })
                .Subscribe(_progressSubj);
        }
    }
}