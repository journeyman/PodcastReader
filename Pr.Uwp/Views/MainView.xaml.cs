using System.Reactive.Disposables;
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

            this.WhenActivated(toDispose =>
            {
                var d = new CompositeDisposable
                {
                    this.BindCommand(ViewModel, x => x.AddSubscriptionCommand, x => x.addSubscriptionButton),
                    this.BindCommand(ViewModel, x => x.LoginToFeedlyCommand, x => x.loginToFeedlyButton)
                };


                list.ItemsSource = ViewModel.Feeds;

                toDispose(d);
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
