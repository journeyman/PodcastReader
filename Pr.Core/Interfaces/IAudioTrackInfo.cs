using System;

namespace Pr.Core.Interfaces
{
    public interface IAudioTrackInfo
    {
        string Title { get; }
        Uri Uri { get; }
        string Artist { get; }
        Uri AlbumArt { get; }
    }
}
