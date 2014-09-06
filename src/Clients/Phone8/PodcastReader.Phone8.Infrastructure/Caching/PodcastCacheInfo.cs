using System;

namespace PodcastReader.Infrastructure.Caching
{
    public class PodcastCacheInfo
    {
        public ulong FinalSize { get; set; }
        public ulong Downloaded { get; set; }
        public Uri FileUri { get; set; }
    }
}