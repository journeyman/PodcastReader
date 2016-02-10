using System;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Interfaces
{
    public interface ISubscriptionsManager
    {
        IObservable<ISubscription> Subscriptions { get; }

        Task ReloadSubscriptions();
        Task AddSubscriptionAsync(ISubscription subscription);
    }
}
