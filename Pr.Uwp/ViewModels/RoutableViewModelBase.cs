using ReactiveUI;
using System;

namespace Pr.Phone8.ViewModels
{
    public abstract class RoutableViewModelBase : ReactiveObject
    {
        public string UrlPathSegment
        {
            get { throw new NotImplementedException(); }
        }

        public IReactiveCommand NavigateCommand { get; }
    }
}
