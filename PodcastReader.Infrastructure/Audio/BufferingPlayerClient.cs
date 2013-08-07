using System;
using System.IO.IsolatedStorage;
using Microsoft.Phone.BackgroundAudio;
using PodcastReader.Infrastructure.Interfaces;
using System.Windows;

namespace PodcastReader.Infrastructure.Audio
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


    public class BufferingPlayerClient : IPlayerClient
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
            string fileName = "Test.mp3";
            var info = Application.GetResourceStream(new Uri("Assets/" + fileName, UriKind.Relative));
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.FileExists("/" + fileName))
                    using (var source = info.Stream)
                        using (var target = iso.CreateFile(fileName))
                            source.CopyTo(target);
            }
            string url = "/" + fileName;
            BackgroundAudioPlayer.Instance.Track = new AudioTrack(null, trackInfo.Title, trackInfo.Artist, null, trackInfo.AlbumArt, url, EnabledPlayerControls.All);
            BackgroundAudioPlayer.Instance.Play();
        }
    }
}
