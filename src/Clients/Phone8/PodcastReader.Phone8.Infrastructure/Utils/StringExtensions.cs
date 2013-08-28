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
    }
}
