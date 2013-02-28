using ReactiveUI;
using ReactiveUI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastReader.Phone8.ViewModels
{
    public class MainViewViewModel : ReactiveObject, IRoutableViewModel
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
