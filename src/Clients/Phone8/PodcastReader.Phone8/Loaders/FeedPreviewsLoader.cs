using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.ServiceModel.Syndication;
using System.Xml;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;

namespace PodcastReader.Phone8.Loaders
{
    public class FeedPreviewsLoader : IFeedPreviewsLoader
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
            //await _subscriptionsManager.ReloadSubscriptions();
            
            var client = new WebClient();
            
            Observable.FromEventPattern<DownloadStringCompletedEventArgs>(client, "DownloadStringCompleted")
                .Select(e => e.EventArgs.Result)
                //DtdProcessing = DtdProcessing.Parse is needed for some feeds (e.g. http://www.dotnetrocks.com/feed.aspx)
                .Select(xml => SyndicationFeed.Load(XmlReader.Create(new StringReader(xml), new XmlReaderSettings{DtdProcessing = DtdProcessing.Ignore})))
                .Select(feed => new FeedModel(feed.Title.Text, new PodcastItemsLoader(feed)))
                .Subscribe(_subject);

            _subscriptionsManager.Subscriptions.Select(s => s.Uri).Subscribe(client.DownloadStringAsync);

            client.DownloadStringAsync(new Uri(TEST_FEED_URL));
        }
    }
}