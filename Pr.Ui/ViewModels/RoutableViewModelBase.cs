using System;
using ReactiveUI;

namespace Pr.Ui.ViewModels
{
    public abstract class RoutableViewModelBase : ReactiveObject
    {
        public string UrlPathSegment
        {
            get { throw new NotImplementedException(); }
        }
    }
}
