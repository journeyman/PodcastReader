using System;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Storage
{
    public interface IBackgroundTransferStorage
    {
        string GetTransferUrl(string relativeUrl);
        Task RemoveFile(Uri downloadLocation);
    }
}
