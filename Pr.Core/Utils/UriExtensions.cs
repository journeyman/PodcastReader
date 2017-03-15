using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pr.Core.Utils
{
    public class QueryParams : Dictionary<string, string> { }

    // based on MvxUriExtensionMethods - MS-Pl
    // in turn, that was based on http://densom.blogspot.com/2009/08/how-to-parse-query-string-without-using.html
    // assumed free to use in the public domain (it's a blog post)
    public static class UriExtensionMethods
    {
        public static string ToUrl(string path, QueryParams query)
        {
            return $"{path}?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))}";
        }

        public static IDictionary<string, string> ParseQueryString(this Uri uri, char divider = '?')
        {
            if (uri == null)
            {
                return new Dictionary<string, string>();
            }

            return Uri.UnescapeDataString(uri.OriginalString).ParseQueryString(divider);
        }

        public static IDictionary<string, string> ParseQueryString(this string url, char divider = '?')
        {
            var toReturn = new Dictionary<string, string>();
            var question = url.IndexOf(divider);
            if (question < 0)
            {
                return toReturn;
            }

            if (question >= (url.Length - 1))
            {
                return toReturn;
            }

            var parameters = url.Substring(question + 1);

            foreach (var vp in Regex.Split(parameters, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    toReturn[singlePair[0]] = singlePair[1].Escaped();
                }
                else
                {
                    toReturn[singlePair[0]] = string.Empty;
                }
            }

            return toReturn;
        }

        public static string ToQueryString(this IDictionary<string, string> nvc, bool encodeValue = true)
        {
            if (encodeValue)
            {
                var array = (from key in nvc.Keys
                             let value = nvc[key]
                             select $"{key.Escaped()}={value.Escaped()}")
                    .ToArray();
                return "?" + string.Join("&", array);
            }
            else
            {
                var array = (from key in nvc.Keys
                             let value = nvc[key]
                             select $"{key.Escaped()}={value}")
                    .ToArray();
                return "?" + string.Join("&", array);
            }
        }

        public static string Escaped(this string str) => Uri.EscapeDataString(str);
    }
}
