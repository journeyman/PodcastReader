using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Akavache;
using Newtonsoft.Json;
using PodcastReader.Infrastructure;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Phone8.Infrastructure
{
    public interface ISubscriptionsCache
    {
        Task<IEnumerable<ISubscription>> LoadSubscriptions();
        Task SaveSubscription(ISubscription subscription);
    }

    public class IsoSubscriptionsCache : ISubscriptionsCache
    {
        private const string CACHE_KEY_FMT = "subscription#{0}";
        private const int DEFAULT_COUNT_VALUE = -1;
        private static int _count = DEFAULT_COUNT_VALUE;

        public async Task<IEnumerable<ISubscription>> LoadSubscriptions()
        {
            var subs = (await Cache.Local.GetAllObjects<SubscriptionDto>()).ToList();
            _count = subs.Count;
            return subs;
        }

        public async Task SaveSubscription(ISubscription subscription)
        {
            Debug.Assert(_count != DEFAULT_COUNT_VALUE, "Count field is not inited, LoadSubscriptions() wasn't called before SaveSubscription()");

            var serializable = new SubscriptionDto {Uri = subscription.Uri};
            await Cache.Local.InsertObject(string.Format(CACHE_KEY_FMT, _count), serializable);
            _count++;
        }
    }

    //[DataContract]
    //[JsonObject]
    public class SubscriptionDto : ISubscription
    {
        //[DataMember]
        //[JsonPropertyAttribute]
        public Uri Uri { get; set; }
    }
}