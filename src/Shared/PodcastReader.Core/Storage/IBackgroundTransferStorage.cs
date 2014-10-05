namespace PodcastReader.Core.Storage
{
    public interface IBackgroundTransferStorage
    {
        string GetTransferUrl(string relativeUrl);
        Task RemoveFile(Uri downloadLocation);
    }
}
