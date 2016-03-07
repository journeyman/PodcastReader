using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web.Syndication;
using Pr.Core.Utils;

namespace Pr.Uwp.Utils
{
    public static class PodcastSyndicationExtensions
    {
        private static string[] SupportedMediaTypes => new[]
        {
	        "mp3",
	        "wav",
	        "mpeg",
	        "audio"//general subscring
        };

	    public static bool IsPodcast(this SyndicationItem This)
        {
            return This.Links.Any(IsLinkToPodcast);
        }

        public static IList<Uri> GetPodcastUris(this SyndicationItem This)
        {
            return This.Links.Where(IsLinkToPodcast)
                             .Select(l => l.Uri)
                             .ToList();
        }

        public static bool IsLinkToPodcast(this SyndicationLink This)
        {
            //MediaType should be something like "audio/mp3"
            return !string.IsNullOrWhiteSpace(This.MediaType) && This.MediaType.ContainsValues(SupportedMediaTypes);
        }
    }
}
