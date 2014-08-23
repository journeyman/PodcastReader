namespace PodcastReader.Infrastructure.Storage
{
    public interface IBackgroundTransferStorage
    {
        string GetTransferUrl(string relativeUrl);
    }
}
