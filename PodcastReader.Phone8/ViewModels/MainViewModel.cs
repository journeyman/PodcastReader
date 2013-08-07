using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IMainViewModel : IRoutableViewModel { }

    public class MainViewModel : RoutableViewModelBase, IMainViewModel
    {
        private readonly IFeedPreviewsLoader _feedPreviews;

        public MainViewModel(IFeedPreviewsLoader feedPreviews)
        {
            _feedPreviews = feedPreviews;

            this.Feeds = feedPreviews.CreateCollection().CreateDerivedCollection(f => f, null, FeedsComparer);
            feedPreviews.Load();
        }

        private int FeedsComparer(IFeedPreview a, IFeedPreview b)
        {
            if (a.LastPublished == b.LastPublished)
                return 0;
            else if (a.LastPublished > b.LastPublished)
                return 1;
            else
                return -1;
        }

        public IReadOnlyReactiveList<IFeedPreview> Feeds { get; private set; }
    }

}
