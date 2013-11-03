using PodcastReader.Phone8.Models;
using ReactiveUI;
using Magellan.WP.Controls;

namespace PodcastReader.Phone8.Views
{
    public partial class FeedDetailsView : Layout, IViewFor<FeedViewModel>
    {
        private FeedViewModel _viewModel;

        public FeedDetailsView()
        {
            InitializeComponent();
        }

        public FeedViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (FeedViewModel)value; } }
    }
}