using System;
using System.Linq;
using System.Reactive.Linq;
using System.ServiceModel.Syndication;
using PodcastReader.Infrastructure.Caching;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Storage;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.ViewModels
{
	public class PodcastItemViewModel : RoutableViewModelBase, IPodcastItem
    {
        private readonly ObservableAsPropertyHelper<ICachingState> _cachingState;

        public PodcastItemViewModel(SyndicationItem item)
        {
            OriginalUri = item.GetPodcastUris().First();
            DatePublished = item.PublishDate;
            Title = item.Title.Text;

            string summary = item.Summary.IfNotNull(its => its.Text) ??
                             item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "summary").IfNotNull(ext => ext.GetObject<string>(), string.Empty);
            
            Summary = summary;
            Id = new PodcastId(this.GetStorageUrl());

            PlayPodcastCommand = ReactiveCommand.Create();
            PlayPodcastCommand.Subscribe(OnPlayPodcast);

            _cachingState = FileCache.Instance.CachedFiles.FirstOrDefaultAsync(x => x.Id == Id).ToProperty(this, x => x.CachingState);
        }

        public PodcastId Id { get; }
        public ICachingState CachingState => _cachingState.Value;
        public DateTimeOffset DatePublished { get; }
        public string Title { get; }
        public string Summary { get; }
        public Uri OriginalUri { get; }
        public Uri PodcastUri => CachingState?.CachedUri ?? OriginalUri;

        public IReactiveCommand<object> PlayPodcastCommand { get; }

        public void OnPlayPodcast(object _)
        {
            var trackUri = OriginalUri;
            //TODO: fix IsFullyCached logic for downloaded in background file
            if (CachingState?.CachedUri != null)
            {
                //fixing to BG player compliant url
                var url = CachingState.CachedUri.OriginalString.TrimStart('/', '\\');
                trackUri = new Uri(url, UriKind.Relative);
            }

            PlayerClient.Default.Play(new PodcastTrackInfo(trackUri, Title, Summary));
        }

        public void OnViewActivated()
        {
            if (CachingState != null)
                return;

            //var downloader = Locator.Current.GetService<IBackgroundDownloader>();
            //var storage = Locator.Current.GetService<IPodcastsStorage>();
            //var cachingState = new CachingState(this, downloader, storage);
            ////TODO: use some heuristics to control expensive caching state initing
            //cachingState.Init();
            //CachingState = new CachingStateVm(cachingState);

            //this.RaisePropertyChanged("CachingState");
        }
    }
}