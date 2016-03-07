using System;
using Pr.Core.Interfaces;
using Splat;

namespace Pr.Phone8.Infrastructure.Audio
{
    public class BackgroundPlayerClient : IPlayerClient, IEnableLogger
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
			throw new NotImplementedException();
            //this.Log().Info("setting to play: {0}", trackInfo.Uri);
            //BackgroundAudioPlayer.Instance.Track = new AudioTrack(trackInfo.Uri,
            //    trackInfo.Title,
            //    trackInfo.Artist,
            //    "Podcasts",
            //    null);
            //BackgroundAudioPlayer.Instance.Play();
        }
    }
}