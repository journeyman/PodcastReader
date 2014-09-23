using System;

namespace PodcastReader.Infrastructure.Interfaces
{
    public interface ISubscription
    {
        Uri Uri { get; }
    }

    public class Subscription : ISubscription
    {
        public Subscription(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; private set; }
    }
}
