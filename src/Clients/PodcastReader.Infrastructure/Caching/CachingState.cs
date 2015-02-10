using System;
using System.Reactive.Linq;
using System.Threading;
using Akavache;
using JetBrains.Annotations;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;

namespace PodcastReader.Infrastructure.Caching
{
    public class CachingState
    {
        [NotNull] private readonly IPodcastItem _item;
        [NotNull] private readonly IBackgroundDownloader _downloader;
        [NotNull] private readonly IPodcastsStorage _storage;

        public CachingState(IPodcastItem item, IBackgroundDownloader downloader, IPodcastsStorage storage)
        {
            _item = item;
            _downloader = downloader;
            _storage = storage;
        }

        public Uri RealUri { get; private set; }

        public IReactiveProgress<ProgressValue> Init()
        {
            var progress = new DeferredReactiveProgress(default(ProgressValue));
            TheInit(progress);
            return progress;
        }

        private async void TheInit([NotNull] DeferredReactiveProgress target)
        {
            var cacheInfo = await Cache.Local.GetObject<PodcastCacheInfo>(_item.OriginalUri.OriginalString)
                .Catch(Observable.Return<PodcastCacheInfo>(null));
            
            if (cacheInfo == null || cacheInfo.Downloaded < cacheInfo.FinalSize)
            {
                //if (not downloaded OR download not finished)
                var progress = new OngoingReactiveProgress1();
                target.SetRealReactiveProgress(progress);
                //TODO: progress.Subscribe( { save CacheInfo as progress goes} );
                var transferUri = await _downloader.Load(_item.PodcastUri.AbsoluteUri, progress, CancellationToken.None);
                //TODO: think how to implement via Move (should atomically call Move and Save info into cache)
                RealUri = await _storage.MoveFromTransferTempStorage(transferUri, _item);
                var newCacheInfo = new PodcastCacheInfo()
                {
                    FileUri = RealUri,
                    FinalSize = progress.FinalState.Total,
                    Downloaded = progress.FinalState.Total
                };
                await Cache.Local.InsertObject(_item.OriginalUri.OriginalString, newCacheInfo);
            }
            else
            {
                RealUri = cacheInfo.FileUri;
                var progress = new FinishedReactiveProgress<ProgressValue>(new ProgressValue(cacheInfo.Downloaded, cacheInfo.FinalSize));
                target.SetRealReactiveProgress(progress);
            }
        }
    }
}