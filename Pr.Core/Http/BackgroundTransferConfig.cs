namespace PodcastReader.Infrastructure.Http
{
    public class BackgroundTransferConfig : IBackgroundTransferConfig
    {
        public BackgroundTransferConfig()
        {
            //Default
            Preferences = PRTransferPreferences.AllowBattery;
#if DEBUG
            Preferences = PRTransferPreferences.AllowCellularAndBattery;
#endif
        }

        public PRTransferPreferences Preferences { get; set; }
    }
}