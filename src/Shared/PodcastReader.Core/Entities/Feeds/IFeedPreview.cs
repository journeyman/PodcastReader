namespace PodcastReader.Core.Entities.Feeds
{
    public interface IFeedPreview
    {
        string Title { get; }
        IFeedItem LastFeedItem { get; }
        DateTimeOffset LatestPublished { get; }
    }
}
