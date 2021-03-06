﻿using System;
using Pr.Core.Interfaces;

namespace Pr.Phone8.Models
{
    public class PodcastTrackInfo : IAudioTrackInfo
    {
        public PodcastTrackInfo(Uri trackUri, string title, string artist)
        {
            Uri = trackUri;
            Title = title;
            Artist = artist;
        }

        public string Title { get; }
        public Uri Uri { get; }
        public string Artist { get; }
        public Uri AlbumArt { get; }
    }
}
