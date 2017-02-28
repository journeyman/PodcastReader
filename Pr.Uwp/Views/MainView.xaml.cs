using Windows.UI.Xaml.Controls;
using Pr.Ui.ViewModels;
using ReactiveUI;

namespace Pr.Uwp.Views
{
    public partial class MainView : UserControl, IViewFor<MainViewModel>
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            ViewModel = viewModel;

            addSubscriptionButton.Click += (sender, args) =>
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
