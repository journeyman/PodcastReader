using System;

namespace PodcastReader.Infrastructure.Interfaces
{
    public interface IAudioTrackInfo
    {
        string Title { get; }
        string Artist { get; }
        Uri AlbumArt { get; }
    }
}
