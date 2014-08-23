using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.BackgroundTransfer;

namespace PodcastReader.Infrastructure.Http
{
    public interface IBackgroundDownloader
    {
        Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation);
    }

    public interface IBackgroundTransferConfig
    {
        TransferPreferences Preferences { get; set; }
    }
}