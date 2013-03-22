using System;
using System.ServiceModel.Syndication;
using Microsoft.Phone.Reactive;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.ViewModels;

namespace PodcastReader.Phone8.Loaders
{
    public class FeedItemsLoader : IFeedItemsLoader
    {
        readonly ISubject<IFeedItem> _subject = new ReplaySubject<IFeedItem>();

        public FeedItemsLoader(SyndicationFeed feed)
        {
            foreach (var syndicationItem in feed.Items)
            {
                _subject.OnNext(new FeedItemViewModel(syndicationItem));
            }
        }

        public IDisposable Subscribe(IObserver<IFeedItem> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
