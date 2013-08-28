using System;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;

namespace PodcastReader.Phone8.Interfaces.Loaders
{
    public interface IFeedPreviewsLoader : IObservable<IFeedPreview>
    {
        void Load();
    }
}