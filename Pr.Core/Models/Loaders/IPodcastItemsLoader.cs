using System;
using Pr.Core.Entities.Podcasts;

namespace Pr.Core.Models.Loaders
{
    public interface IPodcastItemsLoader : IObservable<IPodcastItem>
    {
    }
}