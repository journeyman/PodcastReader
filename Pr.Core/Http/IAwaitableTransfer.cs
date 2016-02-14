using System;
using System.Threading.Tasks;

namespace Pr.Core.Http
{
    public interface IAwaitableTransfer
    {
        Task TransferTask { get; }
        Uri UploadLocation { get; }
        Uri DownloadLocation { get; }
    }
}