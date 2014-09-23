using System;

namespace PodcastReader.Infrastructure.Entities.Podcasts
{
    public struct PodcastId : IEquatable<PodcastId>
    {
        private readonly string _url;

        public PodcastId(string url)
        {
            _url = url;
        }

        public string Url
        {
            get { return _url; }
        }

        public bool Equals(PodcastId other)
        {
            if (string.IsNullOrWhiteSpace(_url)
                || string.IsNullOrWhiteSpace(other._url)
                || _url != other._url)
            {
                return false;
            }
            return true;
        }
    }
}
