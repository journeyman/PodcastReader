using System;
using System.Threading.Tasks;
using PodcastReader.Core.Entities.Podcasts;

namespace PodcastReader.Core.Storage
{
    public interface IPodcastsStorage
    {
        Task<Uri> CopyFromTransferTempStorage(Uri tempUri, IPodcastItem podcast);
    }
}