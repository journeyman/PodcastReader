using System.Collections.Generic;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Infrastructure.Storage
{
    public interface ISubscriptionsCache
    {
        Task<IEnumerable<ISubscription>> LoadSubscriptions();
        Task SaveSubscription(ISubscription subscription);
    }
}