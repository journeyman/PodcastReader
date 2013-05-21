namespace PodcastReader.Infrastructure.Interfaces
{
    public interface IPlayerClient
    {
        void Play(IAudioTrackInfo trackInfo);
    }
}
