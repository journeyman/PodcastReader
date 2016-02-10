using System;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Http
{
    public interface IAwaitableTransfer
    {
        Task TransferTask { get; }
        Uri UploadLocation { get; }
        Uri DownloadLocation { get; }
    }
}