﻿using System;
using System.Reactive.Linq;
using Akavache;
using Microsoft.Phone.Shell;
using PodcastReader.Infrastructure;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.Infrastructure
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
