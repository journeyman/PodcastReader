using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Pr.Uwp.Infrastructure.Services.OAuth;

namespace Pr.Uwp.Views
{
    public sealed partial class BrowserDialog : UserControl, IBrowserPresenter
    {
        public BrowserDialog()
        {
            this.InitializeComponent();
        }

        private void WebBrowser_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            
        }

        private void WebBrowser_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            
        }

        public Task Navigate(Uri uri)
        {
            
        }

        public event Action<Uri> Navigated;
    }
}
