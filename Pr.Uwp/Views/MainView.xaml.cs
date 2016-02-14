using System.Windows.Controls;
using Pr.Phone8.ViewModels;
using ReactiveUI;

namespace Pr.Phone8.Views
{
    public partial class MainView : UserControl, IViewFor<MainViewModel>
    {
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                var binding = this.BindCommand(ViewModel, x => x.AddSubscriptionCommand, x => x.addSubscriptionButton);

                d(binding);
            });
        
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
