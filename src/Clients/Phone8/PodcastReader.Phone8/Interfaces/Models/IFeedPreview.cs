using System;

namespace PodcastReader.Phone8.Interfaces.Models
{
    public interface IFeedPreview
    {
        string Title { get; }
        IFeedItem LastFeedItem { get; }
        DateTimeOffset LatestPublished { get; }
    }
}
