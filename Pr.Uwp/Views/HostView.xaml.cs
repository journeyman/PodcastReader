using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Pr.Core.Utils;

namespace Pr.Uwp.Views
{
    public interface INavigatonStackItem
    {
        UIElement Content { get; }
    }

    public class NavigationStackItem : INavigatonStackItem
    {
        public UIElement Content { get; set; }

        public NavigationStackItem(UIElement content)
        {
            Content = content;
        }
    }

    public interface INavigation
    {
        Task Completion { get; }
    }

    public class Navigation : INavigation
    {
        public Navigation(Task completion)
        {
            Completion = completion;
        }

        public Task Completion { get; }
    }

    public sealed partial class HostView : UserControl
    {
        private readonly AsyncOperationQueue _queue = new AsyncOperationQueue(SynchronizationContext.Current);

        private readonly IDictionary<object, INavigation> _activeNavigations = new Dictionary<object, INavigation>();

        public HostView()
        {
            this.InitializeComponent();
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
