using PodcastReader.FeedsAbstractions.Services;
using ReactiveUI;
using ReactiveUI.Routing;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IMainViewModel : IRoutableViewModel { }

    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly IFeedPreviewsLoader _feedPreviews;

        public string UrlPathSegment
        {
            get { return "main"; }
        }

        public IScreen HostScreen
        {
            get { throw new NotImplementedException(); }
        }

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

        public ReactiveCollection<IFeedPreview> Feeds { get; private set; }
    }

}
