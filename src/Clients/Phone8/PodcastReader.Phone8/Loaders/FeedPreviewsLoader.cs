using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.Loaders
{
    public class FeedLoadingException : Exception
    {
        public FeedLoadingException(Exception inner) : base(string.Empty, inner){}
    }

    public class FeedPreviewsLoader : IFeedPreviewsLoader, IEnableLogger
    {
        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly IObservable<IFeedPreview> _feedsObservable;

        public FeedPreviewsLoader(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;

            var client = new HttpClient();

            var predefinedFeeds = new[]
                                  {
                                      new Uri(TEST_FEED_URL0),
                                      new Uri(TEST_FEED_URL1),
                                      new Uri(TEST_FEED_URL2),
                                      new Uri(TEST_FEED_URL3),
                                      new Uri(TEST_FEED_URL4),
                                  };
            
            _feedsObservable = _subscriptionsManager.Subscriptions
                    .Select(s => s.Uri)
                    .StartWith(predefinedFeeds)
                    .Distinct()
                    .SelectMany(uri => client.GetStringAsync(uri).ToObservable())
                    .Select(FeedXmlParser.Parse)
                    .Select(feed => new FeedViewModel(feed.Title.Text, new PodcastItemsLoader(feed)))
                    .LoggedCatch<IFeedPreview, FeedPreviewsLoader, Exception>(this, ex => Observable.Throw<IFeedPreview>(new FeedLoadingException(ex)));
        }

        private const string TEST_FEED_URL0 = "http://feeds.feedburner.com/Hanselminutes?format=xml";
        private const string TEST_FEED_URL1 = "http://feeds.feedburner.com/netRocksFullMp3Downloads?format=xml";
        private const string TEST_FEED_URL2 = "http://hobbytalks.org/rss.xml";
        private const string TEST_FEED_URL3 = "http://haskellcast.com/feed.xml";
        private const string TEST_FEED_URL4 = "http://thespaceshow.wordpress.com/feed/";

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