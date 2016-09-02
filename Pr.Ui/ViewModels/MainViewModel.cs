using Pr.Ui.Utils.Extensions;
using Pr.Core.Entities.Feeds;
using Pr.Core.Models.Loaders;
using ReactiveUI;
using Splat;

namespace Pr.Ui.ViewModels
{
    public class MainViewModel : RoutableViewModelBase, ISupportsActivation
    {
        private readonly IFeedPreviewsLoader _feedPreviews;

        public MainViewModel(IFeedPreviewsLoader feedPreviews)
        {
            _feedPreviews = feedPreviews;

			AddSubscriptionCommand = NavigateCommand.WithParameter(() => Locator.Current.GetService<AddSubscriptionViewModel>());
			Feeds = feedPreviews.CreateCollection().CreateDerivedCollection(f => f, null, FreshFirstOrderer);

            this.WhenActivated(d => feedPreviews.Load());
        }

        private int FreshFirstOrderer(IFeedPreview a, IFeedPreview b)
        {
	        if (a.LatestPublished > b.LatestPublished)
                return -1;
	        return 1;
        }

	    public IReactiveCommand AddSubscriptionCommand { get; private set; }

        public IReadOnlyReactiveList<IFeedPreview> Feeds { get; private set; }

	    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
