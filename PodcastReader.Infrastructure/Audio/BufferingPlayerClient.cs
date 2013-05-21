using Microsoft.Phone.BackgroundAudio;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Infrastructure.Audio
{
    public class BufferingPlayerClient : IPlayerClient
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
            BackgroundAudioPlayer.Instance.Track = new AudioTrack(null, trackInfo.Title, trackInfo.Artist, null, trackInfo.AlbumArt);
            BackgroundAudioPlayer.Instance.Play();
        }
    }
}
