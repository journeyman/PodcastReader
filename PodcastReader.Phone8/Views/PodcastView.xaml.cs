using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using Magellan.WP.Controls;

namespace PodcastReader.Phone8.Views
{
    public partial class PodcastView : Layout, IViewFor<IFeedItemViewModel>
    {
        private IFeedItemViewModel _viewModel;

        public PodcastView()
        {
            InitializeComponent();
        }

        public IFeedItemViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (IFeedItemViewModel) value; } }
    }
}