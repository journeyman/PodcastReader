using System;

namespace PodcastReader.FeedsAbstractions.Entities
{
    public interface IFeedItem
    {
        DateTimeOffset DatePublished { get; }
        string Title { get; }
        string Summary { get; }
    }

}
