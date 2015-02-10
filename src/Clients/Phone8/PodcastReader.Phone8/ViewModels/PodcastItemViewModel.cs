using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using PodcastReader.Infrastructure.Caching;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.ViewModels
{
    public class StubDownloader : IBackgroundDownloader
    {
        public async Task<Uri> Load(string url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
            url = "https://ia700406.us.archive.org/28/items/GodSaveTheQueen_306/GodSaveTheQueen.ogg";
            var client = new HttpClient();

            var head = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            var totalLength = (ulong)head.Content.Headers.ContentLength;
            var source = await client.GetStreamAsync(url);

            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(Path.GetFileName(url), CreationCollisionOption.ReplaceExisting);

            using (client)
            using (var target = await file.OpenStreamForWriteAsync())
            using (source)
            {
                var buffer = new byte[8192];
                int bytesRead = 0;
                while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await target.WriteAsync(buffer, 0, bytesRead);
                    progress.Report(new ProgressValue((ulong)target.Position, totalLength));
                }
            }
            return new Uri("\\"+Path.GetFileName(url), UriKind.RelativeOrAbsolute);
        }

        public IImmutableDictionary<Uri, IAwaitableTransfer> ActiveRequests
        {
            get { return ImmutableDictionary.Create<Uri, IAwaitableTransfer>(); }
        }

        public Task<IImmutableDictionary<Uri, IAwaitableTransfer>> Update()
        {
            return Task.FromResult(ActiveRequests);
        }

        public Task ForgetAbout(Uri transferUri)
        {
            return Task.FromResult(0);
        }
    }

    public class PodcastItemViewModel : RoutableViewModelBase, IPodcastItem
    {
        private readonly Uri _podcastRemoteUri;

        public PodcastItemViewModel(SyndicationItem item)
        {
            _podcastRemoteUri = item.GetPodcastUris().First();

            DatePublished = item.PublishDate;
            Title = item.Title.Text;

            string summary = item.Summary.IfNotNull(its => its.Text) ??
                             item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "summary").IfNotNull(ext => ext.GetObject<string>(), string.Empty);
            
            Summary = summary;

            PlayPodcastCommand = ReactiveCommand.Create();
            PlayPodcastCommand.Subscribe(OnPlayPodcast);
        }

        public ICachingState CachingState { get; private set; }
        public DateTimeOffset DatePublished { get; }
        public string Title { get; }
        public string Summary { get; }
        public Uri PodcastUri => CachingState?.CachedUri ?? _podcastRemoteUri;

        public IReactiveCommand<object> PlayPodcastCommand { get; }

        public void OnPlayPodcast(object _)
        {
            var trackUri = PodcastUri;
            if (CachingState != null && CachingState.IsFullyCached)
                trackUri = CachingState.CachedUri;

            PlayerClient.Default.Play(new PodcastTrackInfo(trackUri, Title, Summary));
        }

        public void OnViewActivated()
        {
            if (CachingState != null)
                return;

            var downloader = Locator.Current.GetService<IBackgroundDownloader>();
            var storage = Locator.Current.GetService<IPodcastsStorage>();
            var cachingState = new CachingState(this, downloader, storage);
            //TODO: use some heuristics to control expensive caching state initing
            var progress = cachingState.Init();
            CachingState = new CachingStateVm(progress, () => cachingState.RealUri);

            this.RaisePropertyChanged("CachingState");
        }
    }
}