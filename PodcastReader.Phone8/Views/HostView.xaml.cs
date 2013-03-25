using Microsoft.Phone.Controls;
using ReactiveUI;
using ReactiveUI.Routing;

namespace PodcastReader.Phone8.Views
{
    public partial class HostView : PhoneApplicationPage
    {
        public HostView()
        {
            InitializeComponent();

            viewHost.Router = RxApp.GetService<IScreen>().Router;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (viewHost.Router.NavigationStack.Count > 1)
            {
                e.Cancel = true;
                viewHost.Router.NavigateBack.Execute(null);
            }
        }
    }
}