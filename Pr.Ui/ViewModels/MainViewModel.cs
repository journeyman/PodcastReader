using System;
using Pr.Core.Entities.Feeds;
using Pr.Core.Models.Loaders;
using Pr.Ui.Core.OAuth;
using ReactiveUI;

namespace Pr.Ui.ViewModels
{
    public class MainViewModel : RoutableViewModelBase, ISupportsActivation
    {
        private readonly IFeedPreviewsLoader _feedPreviews;

        public MainViewModel(IFeedPreviewsLoader feedPreviews, Authorizer authorizer)
        {
            _feedPreviews = feedPreviews;

            //AddSubscriptionCommand = NavigateCommand.WithParameter(() => Locator.Current.GetService<AddSubscriptionViewModel>());
            LoginToFeedlyCommand = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                try
                {
                    var code = await authorizer.GetCode("https://cloud.feedly.com/subscriptions");
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
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

        public IReactiveCommand LoginToFeedlyCommand { get; set; }

        public IReadOnlyReactiveList<IFeedPreview> Feeds { get; private set; }

	    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
