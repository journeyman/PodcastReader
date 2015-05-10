using PodcastReader.Infrastructure.Caching;
using ReactiveUI;

namespace PodcastReader.Phone8.ViewModels
{
	public class DownloadsViewModel : RoutableViewModelBase
	{
		public DownloadsViewModel()
		{
			Item = FileCache.Instance.CachedFiles.CreateCollection(x => x);
		}

		public IReactiveList<FileModel> Item { get; set; }
	}
}