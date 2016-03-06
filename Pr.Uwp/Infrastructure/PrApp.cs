using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Pr.Core.Caching;
using Pr.Phone8.Infrastructure;
using Pr.Ui.Views;
using Splat;
using Xamarin.Forms;

namespace Pr.Core.App
{
	public class PrApp : Application, IEnableLogger
	{
		public PrApp()
		{
		}

		public static NavigationPage NavigationRoot => (NavigationPage)PrApp.Current.MainPage;

		protected override async void OnStart()
		{
			base.OnStart();

			await InitApp();
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

		private async void OnActivated(bool statePreserved)
		{
			if (!statePreserved)
			{
				await InitApp();
				//RunInitedApp();
			}
		}

		private async Task InitApp()
		{
			BlobCache.ApplicationName = "PodcastReader";

			var b = new AppBootstrapper(); //IoC registrations
			await FileCache.Instance.Init();
		}

		private void RunInitedApp()
		{
			MainPage = new NavigationPage(new MainView());
			//Screen.Router.Navigate.Execute(Locator.Current.GetService<MainViewModel>());
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
