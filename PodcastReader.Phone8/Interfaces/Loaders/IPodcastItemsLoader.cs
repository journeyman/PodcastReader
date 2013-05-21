using System;
using PodcastReader.Phone8.Interfaces.Models;

namespace PodcastReader.Phone8.Interfaces.Loaders
{
    public interface IPodcastItemsLoader : IObservable<IPodcastItem>
    {
    }
}