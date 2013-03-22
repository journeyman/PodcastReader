using System;
using System.ServiceModel.Syndication;
using System.Windows.Input;
using PodcastReader.Phone8.Classes;
using PodcastReader.Phone8.Interfaces.Models;
using ReactiveUI.Routing;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IFeedItemViewModel : IRoutableViewModel { }

    public class FeedItemViewModel : RoutableViewModelBase, IFeedItem, IFeedItemViewModel
    {
        public FeedItemViewModel(SyndicationItem item)
        {
            this.DatePublished = item.PublishDate;
            this.Title = item.Title.Text;
            this.Summary = item.Summary.Text;

            this.GoToPodcastCommand = Screen.Router.Navigate; //ReactiveCommand.Create(_ => true, p => Screen.Router.Navigate.Execute(p));
        }

        public DateTimeOffset DatePublished { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }

        public ICommand GoToPodcastCommand { get; private set; }
    }
}