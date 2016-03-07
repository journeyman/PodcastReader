using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Windows.UI.Xaml;
using Pr.Core.Interfaces;

namespace Pr.Phone8.Infrastructure.Audio
{
    public class BufferingPlayerClient : IPlayerClient
    {
        public void Play(IAudioTrackInfo trackInfo)
        {
			throw new NotImplementedException();
            //string fileName = "Test.mp3";
            //var info = Application.GetResourceStream(new Uri("Assets/" + fileName, UriKind.Relative));
            //using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    if (!iso.FileExists("/" + fileName))
            //        using (var source = info.Stream)
            //            using (var target = iso.CreateFile(fileName))
            //                source.CopyTo(target);
            //}
            //string url = "/" + fileName;
            //BackgroundAudioPlayer.Instance.Track = new AudioTrack(null, trackInfo.Title, trackInfo.Artist, null, trackInfo.AlbumArt, url, EnabledPlayerControls.All);
            //BackgroundAudioPlayer.Instance.Play();
        }
    }
}
