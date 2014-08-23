using System;

namespace PodcastReader.Phone8.Models.Loaders
{
    public class FeedLoadingException : Exception
    {
        public FeedLoadingException(Exception inner) : base(string.Empty, inner){}
    }
}