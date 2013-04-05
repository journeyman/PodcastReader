using System;
using PodcastReader.FeedsAbstractions.Entities;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.Models
{
    public class FeedModel : ReactiveObject, IFeed, IFeedPreview
    {
        private readonly ObservableAsPropertyHelper<IPodcastItem> _lastFeedItemProp;
        private readonly ObservableAsPropertyHelper<DateTimeOffset> _lastPulbishedProp;

        public FeedModel(string title, IPodcastItemsLoader itemsLoader)
        {
            this.Title = title;

            _lastFeedItemProp = new ObservableAsPropertyHelper<IPodcastItem>(itemsLoader, item => this.RaisePropertyChanged(x => x.LastFeedItem));
            _lastPulbishedProp = new ObservableAsPropertyHelper<DateTimeOffset>(_lastFeedItemProp.Select(i => i.LastPublished), dt => this.RaisePropertyChanged(x => x.LastPublished));
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

        public ReactiveCollection<IPodcastItem> Items { get; private set; } 

        public string Title { get; private set; }

        public IFeedItem LastFeedItem
        {
            get { return _lastFeedItemProp.Value; }
        }

        public DateTimeOffset LastPublished
        {
            get { return _ }
        }
    }
}
