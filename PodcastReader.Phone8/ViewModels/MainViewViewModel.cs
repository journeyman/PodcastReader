using ReactiveUI;
using ReactiveUI.Routing;
using System;

namespace PodcastReader.Phone8.ViewModels
{
    public interface IMainViewViewModel : IRoutableViewModel { }

    public class MainViewViewModel : ReactiveObject, IMainViewViewModel
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
