using System;
using System.Threading.Tasks;

namespace Pr.Core.Interfaces
{
    public interface ISubscriptionsManager
    {
        IObservable<ISubscription> Subscriptions { get; }

        Task ReloadSubscriptions();
        Task AddSubscriptionAsync(ISubscription subscription);
    }
}
