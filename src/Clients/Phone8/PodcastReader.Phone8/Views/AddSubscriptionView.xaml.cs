using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using Magellan.WP.Controls;

namespace PodcastReader.Phone8.Views
{
    public partial class AddSubscriptionView : Layout, IViewFor<AddSubscriptionViewModel>
    {
        private AddSubscriptionViewModel _viewModel;

        public AddSubscriptionView()
        {
            InitializeComponent();
        }

        public AddSubscriptionViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (AddSubscriptionViewModel)value; } }
    }
}