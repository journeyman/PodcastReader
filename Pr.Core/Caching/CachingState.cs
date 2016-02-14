using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using JetBrains.Annotations;
using Pr.Core.Entities.Podcasts;
using Pr.Core.Http;
using Pr.Core.Storage;

namespace Pr.Core.Caching
{
    public class DownloadsManager
    {
        private readonly FileCache _fileCache;
        private readonly IBackgroundDownloader _downloader;
        private readonly IStorage _storage;

        public DownloadsManager(FileCache fileCache, IBackgroundDownloader downloader, IStorage storage)
        {
            _fileCache = fileCache;
            _downloader = downloader;
            _storage = storage;
        }

        public IAwaitableTransfer LoadFile(Uri remoteUri)
        {
            var progress = new OngoingReactiveProgress1();
            var transfer = _downloader.Load(remoteUri, progress, CancellationToken.None);
            //TODO: FileCache.TryGetFileModel
            var fileModel = await _fileCache.CachedFiles.FirstOrDefaultAsync();

            if (fileModel != null || cacheInfo.Downloaded < cacheInfo.FinalSize)
            {
                //if (not downloaded OR download not finished)
                var progress = new OngoingReactiveProgress1();
                target.SetRealReactiveProgress(progress);
                //TODO: progress.Subscribe( { save CacheInfo as progress goes} );
                var transfer = await _downloader.Load(_item.PodcastUri, progress, CancellationToken.None);
                //TODO: think how to implement via Move (should atomically call Move and Save info into cache)
                CachedUri = await _storage.MoveFromTransferTempStorage(transfer.DownloadLocation, _item);
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

        public async Task Init()
        {
            await TheInit(_progress).ConfigureAwait(false);
        }

        private async Task TheInit([NotNull] DeferredReactiveProgress target)
        {
            await FileCache.Instance.WaitInit();

            //TODO: FileCache.TryGetFileModel
            var fileModel = await FileCache.Instance.CachedFiles.FirstOrDefaultAsync();
            
            if (fileModel != null || cacheInfo.Downloaded < cacheInfo.FinalSize)
            {
                //if (not downloaded OR download not finished)
                var progress = new OngoingReactiveProgress1();
                target.SetRealReactiveProgress(progress);
                //TODO: progress.Subscribe( { save CacheInfo as progress goes} );
                var transfer = await _downloader.Load(_item.PodcastUri, progress, CancellationToken.None);
                //TODO: think how to implement via Move (should atomically call Move and Save info into cache)
                CachedUri = await _storage.MoveFromTransferTempStorage(transfer.DownloadLocation, _item);
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