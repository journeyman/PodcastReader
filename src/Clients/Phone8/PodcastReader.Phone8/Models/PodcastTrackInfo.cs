using System;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Phone8.Models
{
    public class PodcastTrackInfo : IAudioTrackInfo
    {
        public PodcastTrackInfo(IPodcastItem podcastItem)
        {
            Title = podcastItem.Title;
            Uri = podcastItem.PodcastUri;
            Artist = podcastItem.Summary;//why the hell not?
        }

        public string Title { get; private set; }
        public Uri Uri { get; private set; }
        public string Artist { get; private set; }
        public Uri AlbumArt { get; private set; }
    }
}
