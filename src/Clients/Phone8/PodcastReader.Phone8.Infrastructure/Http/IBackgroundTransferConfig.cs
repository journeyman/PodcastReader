using Microsoft.Phone.BackgroundTransfer;

namespace PodcastReader.Infrastructure.Http
{
    public interface IBackgroundTransferConfig
    {
        TransferPreferences Preferences { get; set; }
    }
}