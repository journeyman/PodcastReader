using Microsoft.Phone.Reactive;
using PodcastReader.FeedsAbstractions.Entities;
using PodcastReader.FeedsAbstractions.Services;
using System;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using PodcastReader.Phone8.Models;

namespace PodcastReader.Phone8.Classes
{
    public class TestFeedsProvider : IFeedsService
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

        public IObservable<IFeed> GetFeedsAsync()
        {
            return this.GetFeeds().Select(syndicationFeed => new FeedModel("blah", new FeedItemsLoader(syndicationFeed)));
        }
    }
}
