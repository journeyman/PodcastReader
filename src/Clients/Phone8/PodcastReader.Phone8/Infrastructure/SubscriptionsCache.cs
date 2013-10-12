using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PodcastReader.Infrastructure.Interfaces;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace PodcastReader.Phone8.Infrastructure
{
    public interface ISubscriptionsCache
    {
        Task<IEnumerable<ISubscription>> LoadSubscriptions();
        Task SaveSubscription(ISubscription subscription);
    }

    public class IsoSubscriptionsCache : ISubscriptionsCache
    {
        private class DeserializedSubscription : ISubscription { public Uri Uri { get; set; } }

        private const string Cache_File_Name = "subscriptionsCache.xml";

        public async Task<IEnumerable<ISubscription>> LoadSubscriptions()
        {
            return await Task.Run(() =>
                {
                    using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = storage.OpenFile(Cache_File_Name, FileMode.OpenOrCreate))
                        {
                            var serializer = new DataContractSerializer(typeof(Uri[]));
                            var uris = (Uri[])serializer.ReadObject(file);
                            return uris.Select(uri => (ISubscription)new DeserializedSubscription {Uri = uri});
                        }
                    }
                })
                .ConfigureAwait(false);
        }

        public async Task SaveSubscription(ISubscription subscription)
        {
            await Task.Run(() =>
                {
                    using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = storage.OpenFile(Cache_File_Name, FileMode.OpenOrCreate))
                        {
                            var serializer = new DataContractSerializer(typeof(Uri));
                            serializer.WriteObject(file, subscription.Uri);
                        }
                    }
                })
                .ConfigureAwait(false);
        }
    }
}