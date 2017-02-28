using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.UI.Xaml;
using Pr.Core.Utils;
using Pr.Uwp.UI.Navigation;

namespace Pr.Uwp.Views
{
    public sealed partial class HostView
    {
        private readonly AsyncOperationQueue _queue = new AsyncOperationQueue(SynchronizationContext.Current);

        private readonly IDictionary<object, INavigation> _activeNavigations = new Dictionary<object, INavigation>();

        public HostView()
        {
            InitializeComponent();
        }

        public IList<INavigatonStackItem> BackStack { get; } = new List<INavigatonStackItem>();

        public INavigation GoBack()
        {
            lock (BackStack)
            {
                if (!BackStack.Any())
                {
                    throw new InvalidOperationException("BackStack is empty, nowhere to go");
                }

                var previous = BackStack[0];
                BackStack.RemoveAt(0);

                return NavigateInternal(previous.Content, false);
            }
        }

        public INavigation Navigate(UIElement item)
        {
            return NavigateInternal(item, true);
        }

        private INavigation NavigateInternal(UIElement item, bool putCurrentIntoBackStack)
        {
            lock (_activeNavigations)
            {
                var navigation = _activeNavigations.TryGet(item);
                if (navigation != null)
                    return navigation;

                var task = _queue.Execute(() =>
                {
                    if (putCurrentIntoBackStack && Container.Child != null)
                    {
                        lock (BackStack)
                        {
                            BackStack.Insert(0, new NavigationStackItem(Container.Child));
                        }
                    }

                    Container.Child = item;
                });

                navigation = new Navigation(task);
                _activeNavigations[item] = navigation;

                return navigation;
            }
        }
    }
}
