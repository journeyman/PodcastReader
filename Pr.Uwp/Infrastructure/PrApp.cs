using System;
using System.Reactive.Linq;
using Akavache;
using Pr.Core.Caching;
using Splat;
using Xamarin.Forms;

namespace Pr.Core.App
{
	public class PrApp : Application, IEnableLogger
	{
		public PrApp()
		{
			MainPage = new NavigationPage();
		}

		public static NavigationPage NavigationRoot => (NavigationPage)PrApp.Current.MainPage;

		protected override void OnStart()
		{
			base.OnStart();

			InitApp();
			RunInitedApp();
		}

		protected override void OnResume()
		{
			base.OnResume();

			OnActivated(false);
		}

		protected override void OnSleep()
		{
			base.OnSleep();

			SaveAppState();
		}

		private void OnActivated(bool statePreserved)
		{
			if (!statePreserved)
			{
				InitApp();
				//RunInitedApp();
			}
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
			Cache.Local.Shutdown.Subscribe(_ => this.Log().Debug("RxUI: shutdown is triggered"));

			//Flushing the CacheIndex to be able to retrieve all inserted keys in batch
			//Calling Shutdown() will break reactivation from Dormant
			Cache.Local.Flush().Wait();
		}
	}
}
