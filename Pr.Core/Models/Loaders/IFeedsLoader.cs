using System;
using Pr.Core.Entities.Feeds;

namespace Pr.Core.Models.Loaders
{
    public interface IFeedsLoader : IObservable<IFeed>
    {
        void LoadFeeds();
    }
}
