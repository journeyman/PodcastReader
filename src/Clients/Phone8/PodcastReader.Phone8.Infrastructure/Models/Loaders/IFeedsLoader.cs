using System;
using PodcastReader.Core.Entities.Feeds;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IFeedsLoader : IObservable<IFeed>
    {
        void LoadFeeds();
    }
}
