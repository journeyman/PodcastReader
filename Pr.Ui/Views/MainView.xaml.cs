using Pr.Ui.ViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace Pr.Ui.Views
{
	public partial class MainView : ContentPage, IViewFor<MainViewModel>
	{
		public MainView(MainViewModel viewModel)
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);

			BindingContext = viewModel;
			ViewModel = viewModel;

			addSubscriptionButton.Clicked += (sender, args) =>
			{
				var count = ViewModel.Feeds.Count;
			};

			this.WhenActivated(toDispose =>
			{
				var binding = this.BindCommand(ViewModel, x => x.AddSubscriptionCommand, x => x.addSubscriptionButton);
				list.ItemsSource = ViewModel.Feeds;

				toDispose(binding);
			});
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MainViewModel)value; }
		}

		public MainViewModel ViewModel { get; set; }
	}
}
