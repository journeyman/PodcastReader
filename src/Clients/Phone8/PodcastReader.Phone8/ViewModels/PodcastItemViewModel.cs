using System;
using System.Linq;
using System.Reactive;
using System.ServiceModel.Syndication;
using PodcastReader.Infrastructure;
using PodcastReader.Infrastructure.Caching;
using PodcastReader.Infrastructure.Entities;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.ViewModels
{
    public class PodcastItemViewModel : RoutableViewModelBase, IPodcastItem
    {
        public PodcastItemViewModel(SyndicationItem item)
        {
            this.DatePublished = item.PublishDate;
            this.Title = item.Title.Text;

            string summary = item.Summary.IfNotNull(its => its.Text) ??
                             item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "summary").IfNotNull(ext => ext.GetObject<string>(), string.Empty);
            
            this.Summary = summary;
            this.PodcastUri = item.GetPodcastUris().First();

            this.PlayPodcastCommand = ReactiveCommand.Create();
            this.PlayPodcastCommand.Subscribe(OnPlayPodcast);

            var cachingState = new CachingState(this, Locator.Current.GetService<IBackgroundDownloader>());
            //TODO: use some heuristics to control expensive caching state initing
            cachingState.Init();
            this.CachingState = cachingState;
        }

        public ICachingState CachingState { get; private set; }

        public DateTimeOffset DatePublished { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public Uri PodcastUri { get; private set; }

        public IReactiveCommand<object> PlayPodcastCommand { get; private set; }

        public void OnPlayPodcast(object _)
        {
            PlayerClient.Default.Play(new PodcastTrackInfo(this));
        }
    }
}