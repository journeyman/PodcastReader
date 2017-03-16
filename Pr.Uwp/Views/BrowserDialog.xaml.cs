using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Pr.Uwp.Infrastructure.Services.OAuth;
using Splat;

namespace Pr.Uwp.Views
{
    public sealed partial class BrowserDialog : UserControl, IBrowserPresenter, IEnableLogger
    {
        private readonly ISubject<Unit> _close = new ReplaySubject<Unit>();

        private readonly ISubject<Uri> _navigating = new Subject<Uri>();

        public BrowserDialog()
        {
            InitializeComponent();

            Navigating = _navigating.TakeUntil(Closed);

            WebBrowser.NavigationCompleted += WebBrowser_OnNavigationCompleted;
            WebBrowser.NavigationStarting += WebBrowser_OnNavigationStarting;
        }

        public Task Navigate(Uri uri)
        {
            WebBrowser.Navigate(uri);
            return Task.CompletedTask;
        }

        public void Close()
        {
            _close.OnNext(Unit.Default);
            _close.OnCompleted();
        }

        public IObservable<Uri> Navigating { get; }

        public IObservable<Unit> Closed => _close;

        private void WebBrowser_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            ProgressBar.IsIndeterminate = false;
        }

        private void WebBrowser_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;

            this.Log().Info("Web browser navigating: " + e.Uri);

            _navigating.OnNext(e.Uri);
        }
    }
}
