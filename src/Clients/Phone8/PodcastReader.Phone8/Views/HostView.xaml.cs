using System.ComponentModel;
using Microsoft.Phone.Controls;
using ReactiveUI;

namespace PodcastReader.Phone8.Views
{
    public partial class HostView : PhoneApplicationPage
    {
        public HostView()
        {
            InitializeComponent();

            viewHost.Router = RxApp.DependencyResolver.GetService<IScreen>().Router;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (viewHost.Router.NavigateBack.CanExecute(null))
            {
                e.Cancel = true;
                viewHost.Router.NavigateBack.Execute(null);
            }
        }
    }
}