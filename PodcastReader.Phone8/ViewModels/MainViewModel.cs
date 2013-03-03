using ReactiveUI;
using ReactiveUI.Routing;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IMainViewModel : IRoutableViewModel { }

    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        public string UrlPathSegment
        {
            get { return "main"; }
        }

        public IScreen HostScreen
        {
            get { throw new NotImplementedException(); }
        }
    }
}
