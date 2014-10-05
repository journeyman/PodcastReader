using System;
using PodcastReader.Core.Entities.Podcasts;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IPodcastItemsLoader : IObservable<IPodcastItem>
    {
    }
}