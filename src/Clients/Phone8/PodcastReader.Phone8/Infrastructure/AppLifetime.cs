using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Microsoft.Phone.Shell;
using PodcastReader.Infrastructure;
using PodcastReader.Infrastructure.Utils.Logging;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using ObservableExtensions = Microsoft.Phone.Reactive.ObservableExtensions;

namespace PodcastReader.Phone8.Infrastructure
{
    public class AppLifetime : IEnableLogger
    {
        public async void OnLaunching()
        {
            InitApp();
            RunInitedApp();
        }

        public async void OnActivated(bool statePreserved)
        {
            if (!statePreserved)
                InitApp();
            RunInitedApp();
        }

        public void OnDeactivated(DeactivationReason reason)
        {
            SaveAppState();
        }

        public void OnClosing()
        {
            SaveAppState();
        }

        private void InitApp()
        {
            var b = new AppBootstrapper(); //IoC registrations
            
            BlobCache.ApplicationName = "PodcastReader";
        }

        private void RunInitedApp()
        {
            Screen.Router.Navigate.Execute(RxApp.DependencyResolver.GetService<MainViewModel>());
        }

        private void SaveAppState()
        {
            Cache.Local.Shutdown.Subscribe( _ =>
                                           {
                                               this.Log().Debug("RxUI: shutdown is triggered");
                                               Debug.WriteLine("shutdown is triggered");
                                           });

            BlobCache.Shutdown().Wait();
        }
    }
}
