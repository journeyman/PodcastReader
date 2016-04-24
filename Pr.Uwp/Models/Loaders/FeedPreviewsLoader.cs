using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Pr.Core.Entities.Feeds;
using Pr.Core.Interfaces;
using Pr.Core.Models.Loaders;
using Pr.Phone8.Infrastructure.Utils;
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

            var client = new HttpClient();

            _feedsObservable = _subscriptionsManager.Subscriptions
                .Select(s => s.Uri)
                .Distinct()
				.SelectMany(uri => client.GetStringAsync(uri).ToObservable())
                .Select(FeedXmlParser.Parse)
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