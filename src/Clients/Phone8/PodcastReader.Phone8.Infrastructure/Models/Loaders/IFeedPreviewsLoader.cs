using System;
using PodcastReader.Infrastructure.Entities.Feeds;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IFeedPreviewsLoader : IObservable<IFeedPreview>
    {
        void Load();
    }
}