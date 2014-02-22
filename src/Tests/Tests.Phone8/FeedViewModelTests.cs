using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using Splat;

namespace Tests.Phone8
{
    [TestClass]
    public class FeedViewModelTests : PR_Test
    {
        readonly HistoricalScheduler _virtualScheduler = new HistoricalScheduler();

        public FeedViewModelTests()
        {
            var proxy = new ProxyResolver(Locator.CurrentMutable);
            proxy.PreventOneRegistrationOf<ICreatesObservableForProperty>(3);
            Locator.CurrentMutable = proxy;

            RxApp.MainThreadScheduler = _virtualScheduler;
        }


        [TestMethod]
        public void Last_FeedItem_Is_The_One_Published_Later()
        {
            var testPodcastsSubj = new ReplaySubject<IPodcastItem>();
            IPodcastItemsLoader testPodcasts = new TestPodcastItemsLoader(testPodcastsSubj);

            var initialDate = DateTime.Now;
            testPodcastsSubj.OnNext(new TestPodcastItem(1, initialDate.AddDays(1)));
            testPodcastsSubj.OnNext(new TestPodcastItem(2, initialDate.AddDays(2)));

            _virtualScheduler.AdvanceBy(TimeSpan.FromSeconds(1));
            var model = new FeedViewModel("TestFeed", testPodcasts);
            _virtualScheduler.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreEqual(2, ((TestPodcastItem)model.LastFeedItem).Id);
            Assert.AreEqual(2, model.Items.Count);

            testPodcastsSubj.OnNext(new TestPodcastItem(3, initialDate.AddDays(3)));
            _virtualScheduler.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreEqual(3, ((TestPodcastItem)model.LastFeedItem).Id);

            testPodcastsSubj.OnNext(new TestPodcastItem(4, initialDate.AddDays(-1)));
            _virtualScheduler.AdvanceBy(TimeSpan.FromSeconds(1));
            
            Assert.AreEqual(3, ((TestPodcastItem)model.LastFeedItem).Id);
        }

        [TestMethod]
        public void Test()
        {
            
        }

        private class TestPodcastItem : IPodcastItem
        {

            public TestPodcastItem(int id, DateTime date)
            {
                Id = id;
                DatePublished = date;
            }

            public int Id { get; set; }
            public DateTimeOffset DatePublished { get; set; }
            public Uri PodcastUri { get; set; }
            public string Title { get; private set; }
            public string Summary { get; private set; }
        }

        private class TestPodcastItemsLoader : IPodcastItemsLoader
        {
            private readonly IObservable<IPodcastItem> _podcasts;

            public TestPodcastItemsLoader(IObservable<IPodcastItem> podcasts)
            {
                _podcasts = podcasts;
            }

            public IDisposable Subscribe(IObserver<IPodcastItem> observer)
            {
                return _podcasts.Subscribe(observer);
            }
        }
    }
}
