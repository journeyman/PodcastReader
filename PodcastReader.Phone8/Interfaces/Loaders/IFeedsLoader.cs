using System;
using PodcastReader.FeedsAbstractions.Entities;

namespace PodcastReader.Phone8.Interfaces.Loaders
{
    public interface IFeedsLoader : IObservable<IFeed>
    {
        void LoadFeeds();
    }
}
