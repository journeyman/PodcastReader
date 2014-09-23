using System;
using PodcastReader.Infrastructure.Entities.Feeds;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IFeedsLoader : IObservable<IFeed>
    {
        void LoadFeeds();
    }
}
