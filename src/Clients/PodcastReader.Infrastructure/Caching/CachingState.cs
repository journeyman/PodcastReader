using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using JetBrains.Annotations;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;

namespace PodcastReader.Infrastructure.Caching
{
    public class FilesLoader
    {
        private readonly IBackgroundDownloader _downloader;
        private readonly IStorage _storage;

        public FilesLoader(IBackgroundDownloader downloader, IStorage storage)
        {
            _downloader = downloader;
            _storage = storage;
        }

        public IAwaitableTransfer LoadFile(Uri remoteUri)
        {
            _downloader.
        }
    }

    public class CachingState
    {
        [NotNull] private readonly DeferredReactiveProgress _progress = new DeferredReactiveProgress(default(ProgressValue));
        [NotNull] private readonly IPodcastItem _item;
        [NotNull] private readonly IBackgroundDownloader _downloader;
        [NotNull] private readonly IPodcastsStorage _storage;

        public CachingState(IPodcastItem item, IBackgroundDownloader downloader, IPodcastsStorage storage)
        {
            _item = item;
            _downloader = downloader;
            _storage = storage;
        }

        public IReactiveProgress<ProgressValue> Progress => _progress;

        public Uri CachedUri { get; private set; }

        public async void Init()
        {
            await TheInit(_progress).ConfigureAwait(false);
        }

        private async Task TheInit([NotNull] DeferredReactiveProgress target)
        {
            var cacheInfo = await Cache.Local.GetObject<CacheInfo>(_item.OriginalUri.OriginalString)
                .Catch(Observable.Return<CacheInfo>(null));
            
            if (cacheInfo == null || cacheInfo.Downloaded < cacheInfo.FinalSize)
            {
                //if (not downloaded OR download not finished)
                var progress = new OngoingReactiveProgress1();
                target.SetRealReactiveProgress(progress);
                //TODO: progress.Subscribe( { save CacheInfo as progress goes} );
                var transferUri = await _downloader.Load(_item.PodcastUri.AbsoluteUri, progress, CancellationToken.None);
                //TODO: think how to implement via Move (should atomically call Move and Save info into cache)
                CachedUri = await _storage.MoveFromTransferTempStorage(transferUri, _item);
                var newCacheInfo = new CacheInfo()
                {
                    FileUri = CachedUri,
                    FinalSize = progress.FinalState.Total,
                    Downloaded = progress.FinalState.Total
                };
                await Cache.Local.InsertObject(_item.OriginalUri.OriginalString, newCacheInfo);
            }
            else
            {
                CachedUri = cacheInfo.FileUri;
                var progress = new FinishedReactiveProgress<ProgressValue>(new ProgressValue(cacheInfo.Downloaded, cacheInfo.FinalSize));
                target.SetRealReactiveProgress(progress);
            }
        }
    }
}