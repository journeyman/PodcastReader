using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.Loaders
{
    public class FeedPreviewsLoader : IFeedPreviewsLoader, IEnableLogger
    {
        private readonly ISubscriptionsManager _subscriptionsManager;

        public FeedPreviewsLoader(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;
        }

        private const string TEST_FEED_URL = "http://feeds.feedburner.com/Hanselminutes?format=xml";

        readonly ISubject<IFeedPreview> _subject = new ReplaySubject<IFeedPreview>();

        public IDisposable Subscribe(IObserver<IFeedPreview> observer)
        {
            return _subject.Subscribe(observer);
        }

        public async void Load()
        {
            var client = new HttpClient();

            _subscriptionsManager.Subscriptions
                .Select(s => s.Uri)
                .StartWith(new Uri(TEST_FEED_URL))
                .Select(uri => client.GetStringAsync(uri).ToObservable())
                .SelectMany(results => results)
                //DtdProcessing = DtdProcessing.Parse is needed for some feeds (e.g. http://www.dotnetrocks.com/feed.aspx)
                .Select(xml =>
                        {
                            using (var reader = XmlReader.Create(new StringReader(xml),new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore}))
                                return SyndicationFeed.Load(reader);
                        })
                .Select(feed => new FeedModel(feed.Title.Text, new PodcastItemsLoader(feed)))
                .Subscribe(_subject);

            await _subscriptionsManager.ReloadSubscriptions();
        }
    }
}