using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using PodcastReader.Infrastructure.Storage;

namespace PodcastReader.Phone8.Infrastructure.Storage
{
    public class WindowsStorage : IStorage
    {
        public async Task Move(string from, string to)
        {
            //var fromPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, from);
            //SS: stupid Path.Combine doesnt work with uris containing "\\"
            var fromPath = ApplicationData.Current.LocalFolder.Path + from;
            var sourceFile = await StorageFile.GetFileFromPathAsync(fromPath);
            var targetFolder = Path.GetDirectoryName(to).TrimStart('\\');
            var targetFileName = Path.GetFileName(to);
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(targetFolder, CreationCollisionOption.OpenIfExists);
            await sourceFile.MoveAsync(folder, targetFileName, NameCollisionOption.FailIfExists);
        }

        public async Task RemoveFile(Uri uri)
        {
            var path = ApplicationData.Current.LocalFolder.Path + uri.OriginalString;
            var file = await StorageFile.GetFileFromPathAsync(path);
            await file.DeleteAsync();
        }
    }
}