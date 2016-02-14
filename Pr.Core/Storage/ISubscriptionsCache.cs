using System.Collections.Generic;
using System.Threading.Tasks;
using Pr.Core.Interfaces;

namespace Pr.Core.Storage
{
    public interface ISubscriptionsCache
    {
        Task<IEnumerable<ISubscription>> LoadSubscriptions();
        Task SaveSubscription(ISubscription subscription);
    }
}