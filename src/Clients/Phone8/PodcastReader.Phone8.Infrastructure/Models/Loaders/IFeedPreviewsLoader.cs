using System;
using PodcastReader.Core.Entities.Feeds;

namespace PodcastReader.Infrastructure.Models.Loaders
{
    public interface IFeedPreviewsLoader : IObservable<IFeedPreview>
    {
        void Load();
    }
}