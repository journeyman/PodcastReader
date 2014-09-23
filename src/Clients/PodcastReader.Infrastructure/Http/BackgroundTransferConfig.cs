namespace PodcastReader.Infrastructure.Http
{
    public class BackgroundTransferConfig : IBackgroundTransferConfig
    {
        public BackgroundTransferConfig()
        {
            //Default
            Preferences = PRTransferPreferences.AllowBattery;
        }

        public PRTransferPreferences Preferences { get; set; }
    }
}