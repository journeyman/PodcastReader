using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Entities.Feeds;
using PodcastReader.Infrastructure.Entities.Podcasts;

namespace PodcastReader.Phone8.Views.Design
{
    public class DesignFeedViewModel : IFeed, IFeedPreview
    {
        public string Title { get; set; }
        public IFeedItem LastFeedItem { get;set; }
        public DateTimeOffset LatestPublished { get; set; }
    }

    public class DesignFeedItem : IPodcastItem
    {
        public DateTimeOffset DatePublished { get; set; }
        public Uri PodcastUri { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
