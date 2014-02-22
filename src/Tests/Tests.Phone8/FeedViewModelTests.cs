using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.Models;
using ReactiveUI;
using ReactiveUI.Xaml;
using Splat;

namespace Tests.Phone8
{
    public class ProxyResolver : IMutableDependencyResolver
    {
        private readonly IDictionary<Type, int> _servicesIndexToFilter = new Dictionary<Type, int>();
        private readonly IDictionary<Type, int> _registeredImplsCount = new Dictionary<Type, int>(); 
        
        private readonly IMutableDependencyResolver _inner;

        public ProxyResolver(IMutableDependencyResolver inner)
        {
            _inner = inner;
        }

        public Func<Type, string, bool> ImplsFilterPredicate { get; set; }

        public void PreventOneRegistrationOf<TService>(int index = 0)
        {
            _servicesIndexToFilter[typeof (TService)] = index;
        }

        private bool shouldFilter(Type impl, string contract)
        {
            return ImplsFilterPredicate != null && ImplsFilterPredicate(impl, contract);
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public object GetService(Type serviceType, string contract = null)
        {
            var impl = _inner.GetService(serviceType, contract);

            if (impl != null)
            {
                if (shouldFilter(impl.GetType(), contract))
                    return null;    
            }
            return impl;
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _inner.GetServices(serviceType, contract).Where(impl =>
                                                                   {
                                                                       var filter = !shouldFilter(impl.GetType(), contract);
                                                                       return filter;
                                                                   });
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            int indexToFilter;
            if (_servicesIndexToFilter.TryGetValue(serviceType, out indexToFilter))
            {
                //incrementing register count
                var countRegisteredImpls = _registeredImplsCount.GetValueOrFallback(serviceType);
                _registeredImplsCount[serviceType] = countRegisteredImpls + 1;

                if (countRegisteredImpls == indexToFilter)
                    return;
            }

            _inner.Register(factory, serviceType, contract);
        }
    }

    [TestClass]
    public class FeedViewModelTests
    {
        readonly HistoricalScheduler _virtualScheduler = new HistoricalScheduler();

        public FeedViewModelTests()
        {
            var detector = new Mock<IModeDetector>();
            detector.Setup(x => x.InUnitTestRunner()).Returns(true);

            ModeDetector.OverrideModeDetector(detector.Object);

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
