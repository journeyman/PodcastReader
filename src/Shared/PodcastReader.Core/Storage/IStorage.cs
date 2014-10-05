namespace PodcastReader.Core.Storage
{
    public interface IStorage
    {
        Task Move(Uri from, Uri to);
        Task RemoveFile(Uri uri);
    }
}