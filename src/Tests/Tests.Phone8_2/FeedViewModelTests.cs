using System;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;

namespace Tests.Phone8_2
{
    [TestClass]
    public class FeedViewModelTests
    {
        [TestMethod]
        public void VerifyProperties1()
        {
            var testPodcastsSubj = new Subject<IPodcastItem>();
            IPodcastItemsLoader testPodcasts = new TestPodcastItemsLoader(testPodcastsSubj);

            var initialDate = DateTime.Now;
            testPodcastsSubj.OnNext(new TestPodcastItem(1, initialDate.AddDays(1)));
            testPodcastsSubj.OnNext(new TestPodcastItem(2, initialDate.AddDays(2)));

            var model = new FeedModel("TestFeed", testPodcasts);

            Assert.Equals(2, model.Items.Count);
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
