using System;
using Pr.Core.Entities.Feeds;

namespace Pr.Core.Models.Loaders
{
    public interface IFeedPreviewsLoader : IObservable<IFeedPreview>
    {
        void Load();
    }
}