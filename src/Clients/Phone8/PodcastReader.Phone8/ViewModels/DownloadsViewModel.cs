using PodcastReader.Infrastructure.Caching;
using ReactiveUI;

namespace PodcastReader.Phone8.ViewModels
{
	public class DownloadsViewModel : RoutableViewModelBase
	{
		public DownloadsViewModel()
		{
			Items = FileCache.Instance.CachedFiles.CreateCollection(x => x);
		}

		public IReactiveDerivedList<FileModel> Items { get; set; }
	}
}