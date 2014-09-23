using System;
using System.IO;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Utils;

namespace PodcastReader.Infrastructure.Storage
{
    public class PodcastsStorage : IPodcastsStorage
    {
        private const string PODCASTS_BASE_PATH = "isostore:/podcasts";
        private readonly IStorage _storage;

        public PodcastsStorage(IStorage storage)
        {
            _storage = storage;
        }

        private static Uri ResolveUriForPodcast(IPodcastItem podcast)
        {
            return new Uri(Path.Combine(PODCASTS_BASE_PATH, podcast.GetSlugName()), UriKind.Absolute);
        }

        public async Task<Uri> CopyFromTransferTempStorage(Uri tempUri, IPodcastItem podcast)
        {
            var targetUri = ResolveUriForPodcast(podcast);
            await _storage.Move(tempUri, targetUri).ConfigureAwait(false);
            return targetUri;
        }
    }
}