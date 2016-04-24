using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pr.Core.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsValues(this string This, string[] values)
        {
            string containsValuesPattern = $"({string.Join("|", values)})";
            return Regex.IsMatch(This, containsValuesPattern);
        }

        public static string ToSlug(this string str)
        {
            //[\x00\x08\x0B\x0C\x0E-\x1F] - means unprintable chars
            return Regex.Replace(str, @"[-\s\x00\x08\x0B\x0C\x0E-\x1F]+", "-");
        }

	    public static string CleanFilePathForSaving(this string filePath)
	    {
		    return filePath.RemoveChars(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).ToArray());
	    }

	    public static string RemoveChars(this string input, char[] chars)
	    {
			var sb = new StringBuilder(input.Length);
		    foreach (var c in input)
		    {
			    if (!chars.Contains(c))
			    {
				    sb.Append(c);
			    }
		    }

		    return sb.ToString();
	    }
    }
}
