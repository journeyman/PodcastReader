using Microsoft.Phone.BackgroundAudio;
using PodcastReader.Infrastructure.Interfaces;
using Splat;

namespace PodcastReader.Phone8.Infrastructure.Audio
{
    public class BackgroundPlayerClient : IPlayerClient, IEnableLogger
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
            this.Log().Info("setting to play: {0}", trackInfo.Uri);
            BackgroundAudioPlayer.Instance.Track = new AudioTrack(trackInfo.Uri,
                trackInfo.Title,
                trackInfo.Artist,
                "Podcasts",
                null);
            BackgroundAudioPlayer.Instance.Play();
        }
    }
}