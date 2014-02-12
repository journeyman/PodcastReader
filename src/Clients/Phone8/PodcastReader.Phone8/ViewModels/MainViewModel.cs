using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.ViewModels
{
    public class MainViewModel : RoutableViewModelBase
    {
        private readonly IFeedPreviewsLoader _feedPreviews;

        public MainViewModel(IFeedPreviewsLoader feedPreviews)
        {
            _feedPreviews = feedPreviews;

            this.AddSubscriptionCommand = HostScreen.Router.Navigate.WithParameter(() => Locator.Current.GetService<AddSubscriptionViewModel>());
            this.Feeds = feedPreviews.CreateCollection().CreateDerivedCollection(f => f, null, FeedsComparer);
            feedPreviews.Load();
        }

        private int FeedsComparer(IFeedPreview a, IFeedPreview b)
        {
            if (a.LastPublished > b.LastPublished)
                return 1;
            else
                return -1;
        }

        public IReactiveCommand AddSubscriptionCommand { get; private set; }

        public IReadOnlyReactiveList<IFeedPreview> Feeds { get; private set; }
    }
}
