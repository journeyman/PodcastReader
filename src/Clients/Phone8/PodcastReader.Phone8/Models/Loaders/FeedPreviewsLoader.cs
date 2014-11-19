using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using PodcastReader.Infrastructure.Entities.Feeds;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Infrastructure.Models.Loaders;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Infrastructure.Utils;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.Models.Loaders
{
    public class FeedPreviewsLoader : IFeedPreviewsLoader, IEnableLogger
    {
        private static readonly string[] TEST_FEEDS = 
            {
                "http://feeds.feedburner.com/Hanselminutes?format=xml",
                "http://feeds.feedburner.com/netRocksFullMp3Downloads?format=xml",
                "http://hobbytalks.org/rss.xml",
                "http://haskellcast.com/feed.xml",
                "http://thespaceshow.wordpress.com/feed/",
        };

        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly IObservable<IFeedPreview> _feedsObservable;

        public FeedPreviewsLoader(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;

            var client = new HttpClient();

            _feedsObservable = _subscriptionsManager.Subscriptions
                .Select(s => s.Uri)
                .StartWith(TEST_FEEDS.Select(x => new Uri(x)))
                .Distinct()
                .SelectManyAndSkipOnException(uri => client.GetStringAsync(uri).ToObservable())
                .SelectAndSkipOnException(FeedXmlParser.Parse)
                .ObserveOnDispatcher()
                .Select(feed => new FeedViewModel(feed.Title.Text, new PodcastItemsLoader(feed)));
        }

        public IDisposable Subscribe(IObserver<IFeedPreview> observer)
        {
            return _feedsObservable.Subscribe(observer);
        }

        public async void Load()
        {
            await _subscriptionsManager.ReloadSubscriptions();
        }
    }
}