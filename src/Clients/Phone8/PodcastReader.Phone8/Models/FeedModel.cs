using System;
using System.Linq;
using System.Reactive.Linq;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.Models
{
    public class FeedModel : ReactiveObject, IFeed, IFeedPreview
    {
        private readonly ObservableAsPropertyHelper<IFeedItem> _lastFeedItemProp;
        private readonly ObservableAsPropertyHelper<DateTimeOffset> _lastPulbishedProp;

        public FeedModel(string title, IPodcastItemsLoader itemsLoader)
        {
            this.Title = title;

            this.Items = itemsLoader.CreateCollection().CreateDerivedCollection(f => f, null, FreshFirstOrderer);

            var lastFeedItemObservable = this.Items.Changed.Select(_ => this.Items.First());
            _lastFeedItemProp = lastFeedItemObservable.ToProperty(this, x => x.LastFeedItem);
            _lastPulbishedProp = lastFeedItemObservable.Select(i => i.DatePublished).ToProperty(this, x => x.LatestPublished);
        }

        private int FreshFirstOrderer(IFeedItem a, IFeedItem b)
        {
            if (a.DatePublished > b.DatePublished)
                return -1;
            else
                return 1;
        }

        public IReadOnlyReactiveList<IPodcastItem> Items { get; private set; } 

        public string Title { get; private set; }

        public IFeedItem LastFeedItem
        {
            get { return _lastFeedItemProp.Value; }
        }

        public DateTimeOffset LatestPublished
        {
            get { return _lastPulbishedProp.Value; }
        }
    }
}
