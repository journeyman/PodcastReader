using PodcastReader.FeedsAbstractions.Entities;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IFeedPreview
    {
        IFeedItem LastFeedItem { get; }
        DateTimeOffset LastPublished { get; }
    }
}
