using System.Windows.Input;
using Microsoft.Phone.Reactive;
using PodcastReader.FeedsAbstractions.Entities;
using System;
using System.ServiceModel.Syndication;
using PodcastReader.Phone8.Classes;
using ReactiveUI;
using ReactiveUI.Xaml;
using ReactiveUI.Routing;

namespace PodcastReader.Phone8.Models
{
    public interface IFeedItemsLoader : IObservable<IFeedItem>
    {
    }

    public interface IFeedItemViewModel : IRoutableViewModel{}

    public class FeedItemViewModel : ReactiveObject, IFeedItem, IFeedItemViewModel
    {
        public string UrlPathSegment
        {
            get { return "podcast"; }
        }

        public IScreen HostScreen
        {
            get { throw new NotImplementedException(); }
        }

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


    public class FeedItemsLoader : IFeedItemsLoader
    {
        readonly ISubject<IFeedItem> _subject = new ReplaySubject<IFeedItem>();

        public FeedItemsLoader(SyndicationFeed feed)
        {
            foreach (var syndicationItem in feed.Items)
            {
                _subject.OnNext(new FeedItemViewModel(syndicationItem));
            }
        }

        public IDisposable Subscribe(IObserver<IFeedItem> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
