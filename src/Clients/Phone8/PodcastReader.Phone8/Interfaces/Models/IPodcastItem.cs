using System;

namespace PodcastReader.Phone8.Interfaces.Models
{
    public interface IPodcastItem : IFeedItem
    {
        Uri PodcastUri { get; }
        string Title { get; }
        string Summary { get; }
    }
}