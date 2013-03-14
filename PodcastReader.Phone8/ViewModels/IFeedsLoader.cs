using PodcastReader.FeedsAbstractions.Entities;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IFeedsLoader : IObservable<IFeed>
    {
        void LoadFeeds();
    }
}
