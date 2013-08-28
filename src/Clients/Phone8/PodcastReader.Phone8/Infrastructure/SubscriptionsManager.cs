using Microsoft.Phone.Reactive;
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

        public void ReloadSubscriptions()
        {
            
        }

        public Task AddSubscriptionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
