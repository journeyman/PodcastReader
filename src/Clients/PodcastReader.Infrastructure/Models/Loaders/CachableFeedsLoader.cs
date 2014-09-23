using System;
using System.Reactive.Linq;
using Akavache;
using PodcastReader.Infrastructure.Entities.Feeds;
using PodcastReader.Infrastructure.Http;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public class CachableFeedsLoader : IFeedPreviewsLoader
    {
        private readonly IFeedPreviewsLoader _inner;

        public CachableFeedsLoader(IFeedPreviewsLoader inner, IBackgroundDownloader downloader, IBlobCache cache)
        {
            _inner = inner;
        }

        public IDisposable Subscribe(IObserver<IFeedPreview> observer)
        {
            return _inner.Select(feed => feed).Subscribe(observer);
        }

        public void Load()
        {
            _inner.Load();
        }
    }
}