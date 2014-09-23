using Microsoft.Phone.BackgroundAudio;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Phone8.Infrastructure.Audio
{
    public class BackgroundPlayerClient : IPlayerClient
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
            BackgroundAudioPlayer.Instance.Track = new AudioTrack(trackInfo.Uri,
                trackInfo.Title,
                trackInfo.Artist,
                "Podcasts",
                null);
            BackgroundAudioPlayer.Instance.Play();
        }
    }
}