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
        public DesignFeedViewModel()
        {
            var date = DateTimeOffset.Now;
            var summary = "fsjfajfj ajf]ja se]fja]sjfsjf] afas fi f a]fj asfp ashfuh as[fh [as faf e shafsef hasvnpisna a";
            var title = "podcast item title";
            var uri = new Uri("http://google.com");

            Items = new []
                {
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                    new DesignFeedItem { DatePublished = date, Title = title, Summary = summary, PodcastUri = uri},
                };
        }

        public string Title { get; set; }
        public IFeedItem LastFeedItem { get;set; }
        public DateTimeOffset LatestPublished { get; set; }
        public IEnumerable<IPodcastItem> Items { get; set; } 
    }

    public class DesignFeedItem : IPodcastItem
    {
        public DateTimeOffset DatePublished { get; set; }
        public Uri PodcastUri { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Uri OriginalUri { get; }
    }
}
