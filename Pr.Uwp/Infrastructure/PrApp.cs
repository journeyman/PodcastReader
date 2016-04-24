using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Pr.Core.Caching;
using Pr.Phone8.Infrastructure;
using Pr.Ui.ViewModels;
using Pr.Ui.Views;
using Splat;
using Xamarin.Forms;

namespace Pr.Core.App
{
	public class PrApp : Application, IEnableLogger
	{
		public static NavigationPage NavigationRoot => (NavigationPage)PrApp.Current.MainPage;

		public PrApp()
		{
			MainPage = new NavigationPage();
		}

		protected override async void OnStart()
		{
			Debug.WriteLine("OnStart");
			base.OnStart();

			await InitApp();
			await RunInitedApp();
		}

		protected override void OnResume()
		{
			Debug.WriteLine("OnResume");
			base.OnResume();

			OnActivated(false);
		}

		protected override void OnSleep()
		{
			Debug.WriteLine("OnSleep");
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

		private async Task RunInitedApp()
		{
			await NavigationRoot.PushAsync(new MainView(Locator.Current.GetService<MainViewModel>()));
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
