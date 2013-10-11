using System.Reactive.Linq;
using System.Reactive.Subjects;
using Akavache;
using PodcastReader.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace PodcastReader.Phone8.Infrastructure
{
    public class SubscriptionsManager : ISubscriptionsManager
    {
        public SubscriptionsManager()
        {
            this.Subscriptions = new ReplaySubject<ISubscription>();
        }

        public IObservable<ISubscription> Subscriptions { get; private set; }

        public async void ReloadSubscriptions()
        {
            var subscriptions = await BlobCache.LocalMachine.GetAllObjects<ISubscription>();
            Subscriptions = subscriptions.ToObservable();
        }

        public async Task AddSubscriptionAsync(ISubscription subscription)
        {
            await BlobCache.LocalMachine.InsertObject("Subscriptions", subscription);
        }
    }
}
