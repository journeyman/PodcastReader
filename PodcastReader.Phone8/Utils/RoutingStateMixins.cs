using System.Reactive.Linq;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace PodcastReader.Phone8.Utils
{
    public static class RoutingStateMixins
    {
        public static IReactiveCommand NavigateCommandForParamOfType<T>(this IRoutingState This)
            where T : IRoutableViewModel
        {
            var ret = new ReactiveCommand(This.Navigate.CanExecuteObservable);
            ret.Select(param => (T)param).InvokeCommand(This.Navigate);
            return ret;
        }
    }
}
