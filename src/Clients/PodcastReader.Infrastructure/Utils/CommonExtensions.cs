using System;

namespace PodcastReader.Infrastructure.Utils
{
    public static class CommonExtensions
    {
        public static TOut IfNotNull<TIn, TOut>(this TIn This, Func<TIn, TOut> selector, TOut fallback = default(TOut))
            where TIn : class
        {
            if (This == null)
                return fallback;
            return selector(This);
        }
    }
}
