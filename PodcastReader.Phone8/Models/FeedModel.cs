using System;
using System.Linq;
using System.Reactive.Linq;
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

            this.Items = itemsLoader.CreateCollection().CreateDerivedCollection(f => f, null, ByDateDescendingComparer);
            _lastFeedItemProp = new ObservableAsPropertyHelper<IPodcastItem>(this.Items.CountChanged.Select(_ => this.Items.Last()), _ => this.RaisePropertyChanged(x => x.LastFeedItem));
            _lastPulbishedProp = new ObservableAsPropertyHelper<DateTimeOffset>(_lastFeedItemProp.Select(i => i.DatePublished), dt => this.RaisePropertyChanged(x => x.LastPublished));
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

        public IReactiveCollection<IPodcastItem> Items { get; private set; } 

        public string Title { get; private set; }

        public IFeedItem LastFeedItem
        {
            get { return _lastFeedItemProp.Value; }
        }

        public DateTimeOffset LastPublished
        {
            get { return _lastPulbishedProp.Value; }
        }
    }
}
