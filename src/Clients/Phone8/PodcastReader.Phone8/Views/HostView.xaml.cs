using System.ComponentModel;
using System.Reactive.Linq;
using Microsoft.Phone.Controls;
using PodcastReader.Phone8.Infrastructure;

namespace PodcastReader.Phone8.Views
{
    public partial class HostView : PhoneApplicationPage
    {
        public HostView()
        {
            InitializeComponent();

            //Resetting ViewContractObservable to prevent view resolution on every SizeChanged (e.g. on page orientation changing)
            //viewHost.ViewContractObservable = Observable.Empty<string>();
            viewHost.Router = Screen.Router;
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