namespace Pr.Core.Http
{
    public interface IBackgroundTransferConfig
    {
        PRTransferPreferences Preferences { get; set; }
    }
}