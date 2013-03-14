using Microsoft.Phone.Reactive;
using PodcastReader.FeedsAbstractions.Entities;
using System;
using System.ServiceModel.Syndication;

namespace PodcastReader.Phone8.Models
{
    public interface IFeedItemsLoader : IObservable<IFeedItem>
    {
    }

    
    public class FeedItemModel : IFeedItem
    {
        public FeedItemModel(SyndicationItem item)
        {
            this.DatePublished = item.PublishDate;
            this.Title = item.Title.Text;
            this.Summary = item.Summary.Text;
        }

        public DateTimeOffset DatePublished { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
    }


    public class FeedItemsLoader : IFeedItemsLoader
    {
        readonly ISubject<IFeedItem> _subject = new ReplaySubject<IFeedItem>();

        public FeedItemsLoader(SyndicationFeed feed)
        {
            foreach (var syndicationItem in feed.Items)
            {
                _subject.OnNext(new FeedItemModel(syndicationItem));
            }
        }

        public IDisposable Subscribe(IObserver<IFeedItem> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
