using System;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Phone.Reactive;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;

namespace PodcastReader.Phone8.Loaders
{
    public class FeedPreviewsLoader : IFeedPreviewsLoader
    {
        private const string TEST_FEED_URL = "http://feeds.feedburner.com/Hanselminutes?format=xml";

        readonly ISubject<IFeedPreview> _subject = new ReplaySubject<IFeedPreview>();

        public IDisposable Subscribe(IObserver<IFeedPreview> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void Load()
        {
            var client = new WebClient();

            Observable.FromEvent<DownloadStringCompletedEventArgs>(client, "DownloadStringCompleted")
                .Select(e => e.EventArgs.Result)
                .Select(xml => SyndicationFeed.Load(XmlReader.Create(new StringReader(xml))))
                .Select(feed => new FeedModel(feed.Title.Text, new PodcastItemsLoader(feed)))
                .Subscribe(_subject);

            client.DownloadStringAsync(new Uri(TEST_FEED_URL));
        }
    }
}