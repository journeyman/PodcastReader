using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Pr.Core.Interfaces;
using Pr.Core.Storage;

namespace Pr.Phone8.Infrastructure
{
    public class SubscriptionsManager : ISubscriptionsManager
    {
        private readonly ISubscriptionsCache _cache;
        private readonly ISubject<ISubscription> _subscriptions;

        public SubscriptionsManager(ISubscriptionsCache cache)
        {
            _cache = cache;

            _subscriptions = new ReplaySubject<ISubscription>();
        }

        public IObservable<ISubscription> Subscriptions => _subscriptions;

        public async Task ReloadSubscriptions()
        {
            var subscriptions = await _cache.LoadSubscriptions().ConfigureAwait(false);

            foreach (var subscription in subscriptions)
                _subscriptions.OnNext(subscription);
        }

        public async Task AddSubscriptionAsync(ISubscription subscription)
        {
            await _cache.SaveSubscription(subscription).ConfigureAwait(false);
            _subscriptions.OnNext(subscription);
        }
    }
}
