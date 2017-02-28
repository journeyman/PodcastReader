using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Pr.Core;
using Pr.Core.Caching;
using Pr.Phone8.Infrastructure;
using Pr.Uwp.Views;
using Splat;

namespace Pr.Uwp.Infrastructure
{
	public class PrApp : IEnableLogger
	{
		public static HostView NavigationRoot { get; private set; }

		public PrApp(HostView hostView)
		{
			BlobCache.ApplicationName = "PodcastReader";
			var b = new AppBootstrapper(); //IoC registrations
			NavigationRoot = hostView;
		}

		public async void OnStart()
		{
			await InitApp();
			await RunInitedApp();
		}

		public void OnResume()
		{
		}

		public void OnSleep()
		{
			SaveAppState();
		}

		private async Task InitApp()
		{
			await FileCache.Instance.Init();
		}

		private async Task RunInitedApp()
		{
			//await NavigationRoot.PushAsync(new MainView(Locator.Current.GetService<MainViewModel>()));
			//Screen.Router.Navigate.Execute(Locator.Current.GetService<MainViewModel>());
		    var view = Locator.Current.GetService<MainView>();
            NavigationRoot.Navigate(view);
		}

		private void SaveAppState()
		{
			Cache.Local.Shutdown.Subscribe(_ => this.Log().Debug("RxUI: shutdown is triggered"));

			//Flushing the CacheIndex to be able to retrieve all inserted keys in batch
			//Calling Shutdown() will break reactivation from Dormant
			Cache.Local.Flush().Wait();
		}
	}
}
