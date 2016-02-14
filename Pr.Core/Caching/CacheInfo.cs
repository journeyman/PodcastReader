using System;

namespace Pr.Core.Caching
{
    public class CacheInfo
    {
        public ulong FinalSize { get; set; }
        public ulong Downloaded { get; set; }
        public Uri FileUri { get; set; }
    }
}