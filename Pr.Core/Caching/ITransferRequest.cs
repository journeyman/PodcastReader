namespace Pr.Core.Caching
{
    public interface ITransferRequest
    {
        IReactiveProgress<ulong?> Progress { get; }
    }
}