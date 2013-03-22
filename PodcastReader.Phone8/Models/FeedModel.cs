using System;
using PodcastReader.FeedsAbstractions.Entities;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.Models
{
    public class FeedModel : ReactiveObject, IFeed, IFeedPreview
    {
        private readonly ObservableAsPropertyHelper<IFeedItem> _lastFeedItemProp;

        public FeedModel(string title, IFeedItemsLoader itemsLoader)
        {
            this.Title = title;

            _lastFeedItemProp = new ObservableAsPropertyHelper<IFeedItem>(itemsLoader, item => this.RaisePropertyChanged(x => x.LastFeedItem));
            this.Items = itemsLoader.CreateCollection().CreateDerivedCollection(f => f, null, ByDateDescendingComparer);
        }

        private int ByDateDescendingComparer(IFeedItem a, IFeedItem b)
        {
            if (a.DatePublished == b.DatePublished)
                return 0;
            else if (a.DatePublished > b.DatePublished)
                return -1;
            else
                return 1;
        }

        public ReactiveCollection<IFeedItem> Items { get; private set; } 

        public string Title { get; private set; }

        public IFeedItem LastFeedItem
        {
            get { return _lastFeedItemProp.Value; }
        }

        public DateTimeOffset LastPublished
        {
            get { return this.LastFeedItem.DatePublished; }
        }
    }
}
