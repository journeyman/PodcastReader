using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using PodcastReader.Infrastructure.Storage;

namespace PodcastReader.Phone8.Infrastructure.Storage
{
    public class WindowsStorage : IStorage
    {
        public async Task Move(Uri from, Uri to)
        {
            var sourceFile = await StorageFile.GetFileFromPathAsync(from.AbsoluteUri);
            var targetFolder = Path.GetFullPath(to.AbsoluteUri);
            var targetFileName = Path.GetFileName(to.AbsoluteUri);
            var folder = await StorageFolder.GetFolderFromPathAsync(targetFolder);
            await sourceFile.MoveAsync(folder, targetFileName, NameCollisionOption.FailIfExists);
        }

        public async Task RemoveFile(Uri uri)
        {
            var file = await StorageFile.GetFileFromPathAsync(uri.AbsoluteUri);
            await file.DeleteAsync();
        }
    }
}