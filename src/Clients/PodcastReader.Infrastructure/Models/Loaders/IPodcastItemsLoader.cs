using System;
using PodcastReader.Infrastructure.Entities.Podcasts;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IPodcastItemsLoader : IObservable<IPodcastItem>
    {
    }
}