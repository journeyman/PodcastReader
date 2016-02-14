using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;
using Pr.Core;
using Pr.Core.Interfaces;
using Pr.Core.Storage;
using Splat;

namespace Pr.Phone8.Infrastructure
{
    public class IsoSubscriptionsCache : ISubscriptionsCache, IEnableLogger
    {
        private const string CACHE_KEY_FMT = "sub{0}";
        private const int DEFAULT_COUNT_VALUE = -1;
        private static int _count = DEFAULT_COUNT_VALUE;

        public async Task<IEnumerable<ISubscription>> LoadSubscriptions()
        {
            var subs = (await Cache.Local.GetAllObjects<string>()).ToList();
            _count = subs.Count;
            return subs.Select(url => new Subscription(new Uri(url, UriKind.Absolute)));
        }

        public async Task SaveSubscription(ISubscription subscription)
        {
            Debug.Assert(_count != DEFAULT_COUNT_VALUE, "Count field is not inited, LoadSubscriptions() wasn't called before SaveSubscription()");

            var serializable = new SubscriptionDto {Uri = subscription.Uri};
            var key = string.Format(CACHE_KEY_FMT, _count);
            this.Log().Info("Inserting subscription to {0} into cache", serializable.Uri);
            await Cache.Local.InsertObject(key, serializable.Uri.OriginalString);
            _count++;
        }
    }
    
    public class SubscriptionDto : ISubscription
    {
        public Uri Uri { get; set; }
    }
}