using Microsoft.Phone.Controls;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;

namespace PodcastReader.Phone8.Views
{
    public partial class MainView : PhoneApplicationPage, IViewFor<MainViewModel>
    {
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.AddSubscriptionCommand, x => x.addSubscriptionButton);
        }

        public MainViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (MainViewModel) value; } }

    }
}