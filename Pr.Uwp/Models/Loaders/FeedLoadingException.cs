using System;

namespace Pr.Phone8.Models.Loaders
{
    public class FeedLoadingException : Exception
    {
        public FeedLoadingException(Exception inner) : base(string.Empty, inner){}
    }
}