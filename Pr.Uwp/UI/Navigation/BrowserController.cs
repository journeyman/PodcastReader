using System;
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
                    _browser.Navigated += uri1 => Navigated?.Invoke(uri1);
                    await PrApp.NavigationRoot.Navigate(browser).Completion;
                    return _browser.Navigate(uri);
                }
                else
                {
                    return _browser.Navigate(uri);
                }
            });
        }

        public event Action<Uri> Navigated;
    }
}
