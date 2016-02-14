using System.Windows.Controls;
using Pr.Phone8.ViewModels;
using ReactiveUI;

namespace Pr.Phone8.Views
{
    public partial class DownloadsView : UserControl, IViewFor<DownloadsViewModel>
    {
        private DownloadsViewModel _viewModel;

        public DownloadsView()
        {
            InitializeComponent();
        }

        public DownloadsViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (DownloadsViewModel)value; } }
    }
}