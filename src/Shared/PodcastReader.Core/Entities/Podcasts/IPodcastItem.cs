using System;
using PodcastReader.Core.Entities.Feeds;

namespace PodcastReader.Core.Entities.Podcasts
{
    public interface IPodcastItem : IFeedItem
    {
        Uri PodcastUri { get; }
        string Title { get; }
        string Summary { get; }
    }
}