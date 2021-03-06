using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Phone8.Infrastructure.Audio
{
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
