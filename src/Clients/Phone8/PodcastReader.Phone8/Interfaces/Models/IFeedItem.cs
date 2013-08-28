using System;

namespace PodcastReader.Phone8.Interfaces.Models
{
    public interface IFeedItem
    {
        DateTimeOffset DatePublished { get; }
    }
}
