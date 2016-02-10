namespace PodcastReader.Infrastructure.Caching
{
    public interface ITransferRequest
    {
        IReactiveProgress<ulong?> Progress { get; }
    }
}