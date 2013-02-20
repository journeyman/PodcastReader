using Microsoft.Phone.Reactive;
using System;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PodcastReader.Phone8.Classes
{
    public interface IFeedsProvider
    {
        IObservable<SyndicationFeed> GetFeeds();
    }

    public class TestFeedsProvider : IFeedsProvider
    {
        private const string TEST_FEED_URL = "http://feeds.feedburner.com/Hanselminutes?format=xml";

        public IObservable<SyndicationFeed> GetFeeds()
        {
            var client = new WebClient();
            var feeds = Observable.FromEvent<DownloadStringCompletedEventArgs>(client, "DownloadStringCompleted")
                .Select(e => e.EventArgs.Result)
                .Select(xml => SyndicationFeed.Load(XmlReader.Create(new StringReader(xml))));

            client.DownloadStringAsync(new Uri(TEST_FEED_URL));

            return feeds;
        }
    }
}
