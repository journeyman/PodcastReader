using PodcastReader.Phone8.Infrastructure;
using ReactiveUI;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public abstract class RoutableViewModelBase : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment
        {
            get { throw new NotImplementedException(); }
        }

        public IScreen HostScreen
        {
            get { return Screen.Instance; }
        }
    }
}
