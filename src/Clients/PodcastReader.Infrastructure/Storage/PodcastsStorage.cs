using System;
using System.IO;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Utils;

namespace PodcastReader.Infrastructure.Storage
{
    public class PodcastsStorage : IPodcastsStorage
    {
        private const string PODCASTS_BASE_PATH = "/podcasts";
        private readonly IStorage _storage;

        public PodcastsStorage(IStorage storage)
        {
            _storage = storage;
        }

        private static string ResolveUriForPodcast(IPodcastItem podcast)
        {
            return Path.Combine(PODCASTS_BASE_PATH, podcast.GetSlugFileName());
        }

        public async Task<Uri> CopyFromTransferTempStorage(Uri tempUri, IPodcastItem podcast)
        {
            var targetPath = ResolveUriForPodcast(podcast);
            await _storage.Move(tempUri.OriginalString, targetPath).ConfigureAwait(false);
            return new Uri(targetPath, UriKind.Relative);
        }
    }
}