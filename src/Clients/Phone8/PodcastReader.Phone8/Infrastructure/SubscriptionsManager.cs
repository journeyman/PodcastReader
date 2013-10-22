using System.Collections.Generic;
using Microsoft.Phone.Reactive;
using PodcastReader.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace PodcastReader.Phone8.Infrastructure
{
    public class SubscriptionsManager : ISubscriptionsManager
    {
        private readonly ISubscriptionsCache _cache;
        private readonly ISubject<ISubscription> _subscriptions;

        public SubscriptionsManager(ISubscriptionsCache cache)
        {
            _cache = cache;

            this._subscriptions = new ReplaySubject<ISubscription>();
        }

        public IObservable<ISubscription> Subscriptions { get { return _subscriptions; } }

        public async Task ReloadSubscriptions()
        {
            var subscriptions = await _cache.LoadSubscriptions();

            foreach (var subscription in subscriptions)
                _subscriptions.OnNext(subscription);
        }

        public async Task AddSubscriptionAsync(ISubscription subscription)
        {
            await _cache.SaveSubscription(subscription);
            _subscriptions.OnNext(subscription);
        }
    }
}
