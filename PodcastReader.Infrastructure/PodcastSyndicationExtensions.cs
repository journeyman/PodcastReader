using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using PodcastReader.Infrastructure.Utils;
using System;

namespace PodcastReader.Infrastructure
{
    public static class PodcastSyndicationExtensions
    {
        private static string[] SupportedMediaTypes
        {
            get
            {
                return new[]
                           {
                               "mp3",
                               "wav"
                           };
            }
        }

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
            return !string.IsNullOrWhiteSpace(This.MediaType) && This.MediaType.ContainsValues(SupportedMediaTypes);
        }
    }
}
