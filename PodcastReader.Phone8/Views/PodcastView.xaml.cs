using Microsoft.Phone.Controls;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using PodcastReader.Phone8.Models;

namespace PodcastReader.Phone8.Views
{
    public partial class PodcastView : PhoneApplicationPage, IViewFor<IFeedItemViewModel>
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