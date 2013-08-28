using System;

namespace PodcastReader.Phone8.Interfaces.Models
{
    public interface IFeedPreview
    {
        IFeedItem LastFeedItem { get; }
        DateTimeOffset LastPublished { get; }
    }
}
