using System;
using PodcastReader.FeedsAbstractions.Entities;

namespace PodcastReader.FeedsAbstractions.Services
{
    public interface IFeedsService
    {
        IObservable<IFeed> GetFeedsAsync();
    }
}
