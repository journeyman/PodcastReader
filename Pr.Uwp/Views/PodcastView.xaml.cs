using Windows.UI.Xaml.Controls;
using Pr.Phone8.ViewModels;
using ReactiveUI;

namespace Pr.Phone8.Views
{
	public partial class PodcastView : UserControl, IViewFor<PodcastItemViewModel>
    {
        private PodcastItemViewModel _viewModel;

        public PodcastView()
        {
            InitializeComponent();

            this.WhenActivated(_ =>
                {
                    ViewModel.OnViewActivated();
                });
        }

        public PodcastItemViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (PodcastItemViewModel) value; } }
    }
}