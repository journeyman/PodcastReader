using Pr.Phone8.ViewModels;
using ReactiveUI;

namespace Pr.Phone8.Views
{
	using System.Windows.Controls;

	public partial class AddSubscriptionView : UserControl, IViewFor<AddSubscriptionViewModel>
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