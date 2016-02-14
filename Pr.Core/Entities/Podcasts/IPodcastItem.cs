using System;
using Pr.Core.Entities.Feeds;

namespace Pr.Core.Entities.Podcasts
{
    public interface IPodcastItem : IFeedItem
    {
        Uri PodcastUri { get; }
        string Title { get; }
        string Summary { get; }
        Uri OriginalUri { get; }
    }
}