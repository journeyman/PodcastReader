using System;
using System.ServiceModel.Syndication;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Loaders;
using PodcastReader.Phone8.Models;
using ReactiveUI;

namespace Tests.UI
{
    [TestClass]
    public class FeedViewModelTests
    {
        [TestMethod]
        public void VerifyProperties()
        {
            //Just registering IScreen
            var testResolver = new FuncDependencyResolver((t, name) => new [] {new TestScreen()});
            testResolver.InitializeResolver();
            RxApp.DependencyResolver = testResolver;

            var items = new[]
                            {
                                new SyndicationItem("1", "a", null) {Summary = new TextSyndicationContent("a")},
                                new SyndicationItem("1", "a", null) {Summary = new TextSyndicationContent("a")},
                            };
            var feed = new SyndicationFeed(items);
            IPodcastItemsLoader testPodcasts = new PodcastItemsLoader(feed);

            //var initialDate = DateTime.Now;
            //testPodcastsSubj.OnNext(new TestPodcastItem(1, initialDate.AddDays(1)));
            //testPodcastsSubj.OnNext(new TestPodcastItem(2, initialDate.AddDays(2)));

            var model = new FeedModel("TestFeed", testPodcasts);

            Assert.AreEqual(2, model.Items.Count);
        }

        private class TestScreen : IScreen
        {
            public IRoutingState Router { get { return new RoutingState(); } }
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
