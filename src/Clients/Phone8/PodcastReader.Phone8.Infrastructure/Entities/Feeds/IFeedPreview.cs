using System;

namespace PodcastReader.Infrastructure.Entities.Feeds
{
    public interface IFeedPreview
    {
        string Title { get; }
        IFeedItem LastFeedItem { get; }
        DateTimeOffset LatestPublished { get; }
    }
}
