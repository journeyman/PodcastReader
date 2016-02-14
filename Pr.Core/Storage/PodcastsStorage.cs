using System;
using System.IO;
using System.Threading.Tasks;
using Pr.Core.Entities.Podcasts;
using Pr.Core.Utils;

namespace Pr.Core.Storage
{
    public static class PodcastUris
    {
        private const string PODCASTS_BASE_PATH = "/podcasts";

        public static string GetStorageUrl(this IPodcastItem podcast)
        {
            return Path.Combine(PODCASTS_BASE_PATH, podcast.GetSlugFileName());
        }
    }

    public class PodcastsStorage : IPodcastsStorage
    {
        private readonly IStorage _storage;

        public PodcastsStorage(IStorage storage)
        {
            _storage = storage;
        }

        public Uri ResolveUriForPodcast(IPodcastItem podcast)
        {
            var path = podcast.GetStorageUrl();
            return new Uri(path, UriKind.Relative);
        }

        public async Task<Uri> MoveFromTransferTempStorage(Uri tempUri, IPodcastItem podcast)
        {
            var targetUri = ResolveUriForPodcast(podcast);
            await _storage.Move(tempUri.OriginalString, targetUri.OriginalString).ConfigureAwait(false);
            return targetUri;
        }
    }
}