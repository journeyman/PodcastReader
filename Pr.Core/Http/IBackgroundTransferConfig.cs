namespace PodcastReader.Infrastructure.Http
{
    public interface IBackgroundTransferConfig
    {
        PRTransferPreferences Preferences { get; set; }
    }
}