using System;

namespace PodcastReader.Infrastructure.Entities.Podcasts
{
    public struct PodcastId : IEquatable<PodcastId>
    {
        public PodcastId(string url)
        {
            Url = url;
        }

        public string Url { get; }

        public bool Equals(PodcastId other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PodcastId && Equals((PodcastId) obj);
        }

        public static bool operator ==(PodcastId a, PodcastId b)
        {
            return !string.IsNullOrWhiteSpace(a.Url) && !string.IsNullOrWhiteSpace(b.Url) && a.Url == b.Url;
        }

        public static bool operator !=(PodcastId a, PodcastId b)
        {
            return !(a == b);
        }
        
        public override int GetHashCode()
        {
            return Url?.GetHashCode() ?? 0;
        }
    }
}
