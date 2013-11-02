using System;
using System.Text.RegularExpressions;

namespace PodcastReader.Infrastructure.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsValues(this string This, string[] values)
        {
            string containsValuesPattern = string.Format("({0})", string.Join("|", values));
            return Regex.IsMatch(This, containsValuesPattern);
        }

        public static TOut IfNotNull<TIn, TOut>(this TIn This, Func<TIn, TOut> select, TOut fallback = default(TOut))
            where TIn : class
        {
            if (This == null)
                return fallback;
            return select(This);
        }
    }
}
