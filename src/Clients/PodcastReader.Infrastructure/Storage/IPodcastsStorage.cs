using System;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Entities.Podcasts;

namespace PodcastReader.Infrastructure.Storage
{
    public interface IPodcastsStorage
    {
        Task<Uri> MoveFromTransferTempStorage(Uri tempUri, IPodcastItem podcast);
        Uri ResolveUriForPodcast(IPodcastItem podcast);
    }
}