using System;
using System.Linq;
using System.ServiceModel.Syndication;
using PodcastReader.Infrastructure;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;
using ReactiveUI;

namespace PodcastReader.Phone8.ViewModels
{
    public class PodcastItemViewModel : RoutableViewModelBase, IPodcastItem
    {
        public PodcastItemViewModel(SyndicationItem item)
        {
            this.DatePublished = item.PublishDate;
            this.Title = item.Title.Text;
            this.Summary = item.Summary.Text;
            this.PodcastUri = item.GetPodcastUris().First();


            this.GoToPodcastCommand = Screen.Router.Navigate; //Screen.Router.NavigateCommandForParamOfType<IPodcastItemViewModel>(); //ReactiveCommand.Create(_ => true, p => Screen.Router.Navigate.Execute(p));
            this.PlayPodcastCommand = new ReactiveCommand();
            this.PlayPodcastCommand.Subscribe(OnPlayPodcast);
        }

        public DateTimeOffset DatePublished { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public Uri PodcastUri { get; private set; }

        public IReactiveCommand GoToPodcastCommand { get; private set; }
        public IReactiveCommand PlayPodcastCommand { get; private set; }

        public void OnPlayPodcast(object _)
        {
            PlayerClient.Default.Play(new PodcastTrackInfo(this));
        }
    }
}