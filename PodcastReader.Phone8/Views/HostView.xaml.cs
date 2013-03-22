using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveUI;
using ReactiveUI.Routing;

namespace PodcastReader.Phone8.Views
{
    public partial class HostView : PhoneApplicationPage
    {
        public HostView()
        {
            InitializeComponent();
            this.
            viewHost.Router = RxApp.GetService<IScreen>().Router;
        }
    }
}