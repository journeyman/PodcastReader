using System;

namespace PodcastReader.Core.Entities.Feeds
{
    public interface IFeedItem
    {
        DateTimeOffset DatePublished { get; }
    }
}
