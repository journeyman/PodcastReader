using System.IO;
using Pr.Core.Entities.Podcasts;

namespace Pr.Core.Utils
{
	public static class PrExtensions
	{
		public static string GetSlugFileName(this IPodcastItem podcast)
		{
			var date = podcast.DatePublished.ToString("yyyy-mm-dd");
			var title = podcast.Title.CleanFilePathForSaving().ToSlug();
			var ext = Path.GetExtension(podcast.OriginalUri.OriginalString);
			return $"{date}-{title}{ext}";
		}
	}
}