using System;
using PodcastReader.Infrastructure.Entities.Feeds;

namespace PodcastReader.Infrastructure.Entities.Podcasts
{
    public interface IPodcastItem : IFeedItem
    {
        Uri PodcastUri { get; }
        string Title { get; }
        string Summary { get; }
        Uri OriginalUri { get; }
    }
}