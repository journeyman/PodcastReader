using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PodcastReader.Infrastructure.Models.Loaders;
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
            //proxy.PreventOneRegistrationOf<ICreatesObservableForProperty>(3);
            proxy.PreventOneRegistrationOf<ICreatesCommandBinding>(0);
            
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
        public void CollectionCountChangedTest()
        {
            var fixture = new ReactiveList<int>();
            var before_output = new List<int>();
            var output = new List<int>();

            fixture.CountChanging.Subscribe(before_output.Add);
            fixture.CountChanged.Subscribe(output.Add);

            fixture.Add(10);
            fixture.Add(20);
            fixture.Add(30);
            fixture.RemoveAt(1);
            fixture.Clear();

            var before_results = new[] { 0, 1, 2, 3, 2 };
            Assert.AreEqual(before_results.Length, before_output.Count);

            var results = new[] { 1, 2, 3, 2, 0 };
            Assert.AreEqual(results.Length, output.Count);
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
