using System;
using System.Reactive.Linq;
using Akavache;
using PodcastReader.Infrastructure;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using ReactiveUI;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IReactiveProgress<out T>
    {
        T FinalState { get; }
        IObservable<T> Values { get; } 
    }

    public interface ITransferRequest
    {
        IReactiveProgress<ulong?> Progress { get; }
    }

    public interface ICachingState : IReactiveObject
    {
        ulong? CachedSize { get; }
        ulong? FinalSize { get; }
        bool IsFullyCached { get; }
        IReactiveProgress<ulong> Progress { get; }
    }

    public class CachingState : ReactiveObject, ICachingState
    {
        private readonly IPodcastItem _item;
        private readonly IBackgroundDownloader _downloader;

        public CachingState(IPodcastItem item, IBackgroundDownloader downloader)
        {
            _item = item;
            _downloader = downloader;
            
        }

        public IReactiveProgress<ulong> Progress { get; private set; }
        public ulong? CachedSize { get; private set; }
        public ulong? FinalSize { get; private set; }
        public bool IsFullyCached { get; private set; }

        public void Init()
        {
            var cacheInfo = Cache.Local.GetObject<PodcastCacheInfo>(_item.PodcastUri.AbsoluteUri)
                .Catch(Observable.Empty<PodcastCacheInfo>())
                .FirstOrDefaultAsync();


            //TODO: concat cacheInfo with:
            // 1. empty if already cached
            // 2. caching progress of existing transfer
            // 3. caching progress of new transfer




            //var progress = itemDownloadRequest.Progress;
            //progress.Values
            //    .Select(_ => progress.FinalState)
            //    .Where(finalState => finalState.HasValue)
            //    .FirstAsync()
            //    .Subscribe(size => FinalSize = size);
        }
    }

    public class PodcastCacheInfo
    {
        public ulong FinalSize { get; set; }
        public ulong Downloaded { get; set; }
        public Uri FileUri { get; set; }
    }
}