using System;
using Pr.Ui.Core.Navigation;
using ReactiveUI;

namespace Pr.Ui.ViewModels
{
    public abstract class RoutableViewModelBase : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment
        {
            get { throw new NotImplementedException(); }
        }

	    public IScreen HostScreen => Screen.Current;

	    public IReactiveCommand NavigateCommand => HostScreen.Router.Navigate;
    }
}
