using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PodcastReader.Phone8
{
    public static class RoutableViewModelExtensions
    {
        public static IReactiveCommand WithParameter<TParam>(this INavigateCommand This, Func<TParam> getParam)
        {
            var command = new ReactiveCommand(This.CanExecuteObservable);
            command.Select(_ => getParam()).InvokeCommand(This);
            return command;
        }
    }
}
