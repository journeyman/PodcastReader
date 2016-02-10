using System;

namespace PodcastReader.Infrastructure.Entities.Feeds
{
    public interface IFeedItem
    {
        DateTimeOffset DatePublished { get; }
    }
}
