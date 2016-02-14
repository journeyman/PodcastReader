using System.ComponentModel;
using System.Reactive.Linq;
using Microsoft.Phone.Controls;
using Pr.Phone8.Infrastructure;

namespace Pr.Phone8.Views
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
            //TODO:SS: hotfix as NavigationStack.CountChanged doesn't work (why?)
            //if (viewHost.Router.NavigateBack.CanExecute(null))
            if (viewHost.Router.NavigationStack.Count > 1)
            {
                e.Cancel = true;
                viewHost.Router.NavigateBack.Execute(null);
            }
        }
    }
}