using System.Text.RegularExpressions;

namespace Pr.Core.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsValues(this string This, string[] values)
        {
            string containsValuesPattern = string.Format("({0})", string.Join("|", values));
            return Regex.IsMatch(This, containsValuesPattern);
        }

        public static string ToSlug(this string str)
        {
            //[\x00\x08\x0B\x0C\x0E-\x1F] - means unprintable chars
            return Regex.Replace(str, @"[-\s\x00\x08\x0B\x0C\x0E-\x1F]+", "-");
        }
    }
}
