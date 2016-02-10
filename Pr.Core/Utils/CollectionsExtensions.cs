using System.Collections.Generic;

namespace PodcastReader.Infrastructure.Utils
{
    public static class CollectionsExtensions
    {
        public static TValue GetValueOrFallback<TKey, TValue>(this IDictionary<TKey, TValue> This, TKey key, TValue fallback = default(TValue))
        {
            TValue val;
            if (!This.TryGetValue(key, out val))
                return fallback;
            return val;
        }
    }
}