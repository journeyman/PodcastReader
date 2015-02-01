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

        public Uri ResolveUriForPodcast(IPodcastItem podcast)
        {
            var path = Path.Combine(PODCASTS_BASE_PATH, podcast.GetSlugFileName());
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