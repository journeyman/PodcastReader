using System;

namespace Pr.Core.Entities.Feeds
{
    public interface IFeedItem
    {
        DateTimeOffset DatePublished { get; }
    }
}
