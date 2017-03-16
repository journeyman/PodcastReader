using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Pr.Core.Utils;
using Pr.Uwp.Infrastructure;
using Pr.Uwp.Infrastructure.Services.OAuth;
using Pr.Uwp.Views;

namespace Pr.Uwp.UI.Navigation
{
    public class BrowserController : IBrowserPresenter
    {
        private readonly AsyncOperationQueue _queue;
        private readonly ISubject<Unit> _close = new ReplaySubject<Unit>();
        private readonly ISubject<Uri> _navigating = new Subject<Uri>();
        private IBrowserPresenter _browser;

        public BrowserController(SynchronizationContext syncContext)
        {
            _queue = new AsyncOperationQueue(syncContext);
        }

        public Task Navigate(Uri uri)
        {
            return _queue.Execute(async () =>
            {
                if (_browser == null)
                {
                    var browser = new BrowserDialog();
                    _browser = browser;
                    //TODO: manage _navigating and _close
                    await PrApp.NavigationRoot.Navigate(browser).Completion;
                    return _browser.Navigate(uri);
                }
                else
                {
                    return _browser.Navigate(uri);
                }
            });
        }

        public void Close()
        {
            _browser.Close();
            PrApp.NavigationRoot.GoBack();
        }

        public IObservable<Uri> Navigating => _navigating;

        public IObservable<Unit> Closed => _close;
    }
}
