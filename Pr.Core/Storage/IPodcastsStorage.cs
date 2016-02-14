using System;
using System.Threading.Tasks;
using Pr.Core.Entities.Podcasts;

namespace Pr.Core.Storage
{
    public interface IPodcastsStorage
    {
        Task<Uri> MoveFromTransferTempStorage(Uri tempUri, IPodcastItem podcast);
        Uri ResolveUriForPodcast(IPodcastItem podcast);
    }
}