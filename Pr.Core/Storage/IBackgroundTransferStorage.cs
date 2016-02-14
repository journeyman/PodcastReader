using System;
using System.Threading.Tasks;

namespace Pr.Core.Storage
{
    public interface IBackgroundTransferStorage
    {
        string GetTransferUrl(string relativeUrl);
        Task RemoveFile(Uri downloadLocation);
    }
}
