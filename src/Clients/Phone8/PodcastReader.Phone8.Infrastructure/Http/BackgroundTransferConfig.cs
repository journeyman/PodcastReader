using Microsoft.Phone.BackgroundTransfer;

namespace PodcastReader.Infrastructure.Http
{
    public class BackgroundTransferConfig : IBackgroundTransferConfig
    {
        public BackgroundTransferConfig()
        {
            //Default
            Preferences = TransferPreferences.AllowBattery;
        }

        public TransferPreferences Preferences { get; set; }
    }
}