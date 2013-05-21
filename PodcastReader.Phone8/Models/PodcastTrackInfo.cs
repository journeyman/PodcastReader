using System;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Phone8.Interfaces.Models;

namespace PodcastReader.Phone8.Models
{
    public class PodcastTrackInfo : IAudioTrackInfo
    {
        public PodcastTrackInfo(IPodcastItem podcastItem)
        {
            Title = podcastItem.Title;
        }

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public Uri AlbumArt { get; private set; }
    }
}
