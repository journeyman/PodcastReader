using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PodcastReader.Infrastructure.Models.Loaders;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;

namespace Tests.Phone
{
    [TestClass]
    public class Navigation_Tests : PR_Test
    {
        [TestMethod]
        public void After_Two_Navigates_I_Am_Able_To_Go_Back()
        {
            var uiTask = Observable.Start(() =>
            {
                var app = new AppBootstrapper();
                //var app = new AppLifetime();
                //app.OnLaunching();
                //var vm = Locator.Current.GetService<MainViewModel>();
                var vm = new TestVM();
                Screen.Router.Navigate.Execute(vm);
                Screen.Router.Navigate.Execute(vm);
                //vm.AddSubscriptionCommand.Execute(null);

                Screen.Router.NavigationStack.ItemsAdded.Take(2).Wait();

                var canGoBack = Screen.Router.NavigateBack.CanExecute(null);

                Assert.IsTrue(canGoBack);

            }, RxApp.MainThreadScheduler).FirstOrDefaultAsync();

            uiTask.Wait();
        }


        [TestMethod]
        public void Test()
        {
            var b = new AppBootstrapper();
            var router = Screen.Router;

            router.Navigate.Execute(new TestVM());
            router.Navigate.Execute(new TestVM());

            router.NavigationStack.ItemsAdded.Take(2).Wait();

            Assert.AreEqual(2, router.NavigationStack.Count, "number of NavStack entries is unexpected");
            var canGoBack = Screen.Router.NavigateBack.CanExecute(null);
            Assert.IsTrue(canGoBack, "Cant go back");
        }

        public class TestVM : RoutableViewModelBase { }

        public class TestFeed : IFeedPreview
        {
            public string Title { get; private set; }
            public IFeedItem LastFeedItem { get; private set; }
            public DateTimeOffset LatestPublished { get; private set; }
        }

        public class TestFeedsLoader : IFeedPreviewsLoader
        {
            private readonly IEnumerable<IFeedPreview> _feeds;

            public TestFeedsLoader(IEnumerable<IFeedPreview> feeds)
            {
                _feeds = feeds;
            }

            public IDisposable Subscribe(IObserver<IFeedPreview> observer)
            {
                return _feeds.ToObservable().Subscribe(observer);
            }

            public void Load()
            {
            }
        }
    }
}
