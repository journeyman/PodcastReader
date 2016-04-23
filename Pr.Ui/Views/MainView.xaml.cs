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

			BindingContext = viewModel;
			ViewModel = viewModel;

			this.WhenActivated(d =>
			{
				var binding = this.BindCommand(ViewModel, x => x.AddSubscriptionCommand, x => x.addSubscriptionButton);

				d(binding);
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
