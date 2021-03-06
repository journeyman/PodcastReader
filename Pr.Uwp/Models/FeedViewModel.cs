﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Pr.Core.Utils;
using Pr.Core.Entities.Feeds;
using Pr.Core.Entities.Podcasts;
using Pr.Core.Models.Loaders;
using Pr.Phone8.ViewModels;
using Pr.Ui.ViewModels;
using ReactiveUI;

namespace Pr.Phone8.Models
{
    public class FeedViewModel : RoutableViewModelBase, IFeed, IFeedPreview
    {
        private readonly ObservableAsPropertyHelper<IFeedItem> _lastFeedItemProp;
        private readonly ObservableAsPropertyHelper<DateTimeOffset> _lastPulbishedProp;

        public FeedViewModel(string title, IPodcastItemsLoader itemsLoader)
        {
            this.Title = title;
            this.Items = itemsLoader.CreateCollection().CreateDerivedCollection(f => f, null, FreshFirstOrderer);
            
            var lastFeedItemObservable = Items.Changed.Select(_ => this.Items.FirstOrDefault());
            _lastFeedItemProp = lastFeedItemObservable.ToProperty(this, x => x.LastFeedItem, Items.FirstOrDefault());
            _lastPulbishedProp = lastFeedItemObservable
                .Select(i => i.DatePublished)
                .ToProperty(this, x => x.LatestPublished, Items.FirstOrDefault().IfNotNull(i => i.DatePublished));
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

        public IFeedItem LastFeedItem => _lastFeedItemProp.Value;

	    public DateTimeOffset LatestPublished => _lastPulbishedProp.Value;
    }
}
