using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Pr.Core.Interfaces;
using Pr.Core.Storage;
using ReactiveUI;
using Splat;

namespace Pr.Core.Utils
{
    public static class DebugState
    {
        private static readonly string[] TEST_FEEDS =
            {
                "http://feeds.feedburner.com/Hanselminutes?format=xml",
                "http://feeds.feedburner.com/netRocksFullMp3Downloads?format=xml",
                "http://hobbytalks.org/rss.xml",
                "http://haskellcast.com/feed.xml",
                "http://thespaceshow.wordpress.com/feed/",
            };
        
        [Conditional("DEBUG")]
        public static void Set()
        {
            var saveTask = Task.Run(async () =>
                {
                    var subsCache = Locator.Current.GetService<ISubscriptionsCache>();
                    var saved = (await subsCache.LoadSubscriptions()).ToList();

                    var savings = TEST_FEEDS
                        .Select(x => new Subscription(new Uri(x)))
                        .Except(saved)
                        .ToList();

                    foreach (var sub in savings)
                    {
                        await subsCache.SaveSubscription(sub);
                    }
                });

            saveTask.Wait();
        }
    }
}
