using Xamarin.Forms;

namespace Pr.Core.App
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage();
		}

		public static NavigationPage NavigationRoot => (NavigationPage)App.Current.MainPage;
	}
}
