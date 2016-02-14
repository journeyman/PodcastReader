using System;
using System.Reactive.Linq;
using Akavache;
using Microsoft.Phone.Shell;
using Pr.Core;
using Pr.Core.Caching;
using Pr.Phone8.ViewModels;
using Splat;

namespace Pr.Phone8.Infrastructure
{
    public class AppLifetime : IEnableLogger
    {
        public void OnLaunching()
        {
            InitApp();
            RunInitedApp();
        }

        public void OnActivated(bool statePreserved)
        {
            if (!statePreserved)
            {
                InitApp();
                //RunInitedApp();
            }
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
            BlobCache.ApplicationName = "PodcastReader";

            var b = new AppBootstrapper(); //IoC registrations
			FileCache.Instance.Init();
        }

        private void RunInitedApp()
        {
            Screen.Router.Navigate.Execute(Locator.Current.GetService<MainViewModel>());
        }

        private void SaveAppState()
        {
            Cache.Local.Shutdown.Subscribe( _ => this.Log().Debug("RxUI: shutdown is triggered"));

            //Flushing the CacheIndex to be able to retrieve all inserted keys in batch
            //Calling Shutdown() will break reactivation from Dormant
            Cache.Local.Flush().Wait();
        }
    }
}
