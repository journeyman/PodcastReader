using System;

namespace PodcastReader.FeedsAbstractions.Entities
{
    public interface IPodcastItem : IFeedItem
    {
        Uri PodcastUri { get; }
    }
}