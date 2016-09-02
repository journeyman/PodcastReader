using System;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using Pr.Core.Entities.Feeds;
using Pr.Core.Interfaces;
using Pr.Core.Models.Loaders;
using Pr.Core.Utils;
using Pr.Phone8.Infrastructure.Utils;
using ReactiveUI;
using Splat;

namespace Pr.Phone8.Models.Loaders
{
    public class FeedPreviewsLoader : IFeedPreviewsLoader, IEnableLogger
    {
        

        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly IObservable<IFeedPreview> _feedsObservable;

        public FeedPreviewsLoader(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;

	        var handler = new HttpClientHandler
	        {
		        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
	        };
            var client = new HttpClient(handler);

            _feedsObservable = _subscriptionsManager.Subscriptions
                .Select(s => s.Uri)
                .Distinct()
				.SelectMany(async url =>
				{
					this.Log().Debug($"loading {url}");
					var response = await client.GetStringAsync(url);
					this.Log().Debug($"parsing {url}");
					return FeedXmlParser.Parse(response);
				})
#if !DEBUG
				.SkipOnException()
#endif
				.ObserveOnDispatcher()
                .Select(feed => new FeedViewModel(feed.Title.Text, new PodcastItemsLoader(feed)));
        }

        public IDisposable Subscribe(IObserver<IFeedPreview> observer)
        {
            return _feedsObservable.Subscribe(observer);
        }

        public async void Load()
        {
            await _subscriptionsManager.ReloadSubscriptions();
        }
    }
}