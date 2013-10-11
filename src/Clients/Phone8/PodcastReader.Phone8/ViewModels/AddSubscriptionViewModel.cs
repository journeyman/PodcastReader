using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using System.Windows.Input;
using PodcastReader.Infrastructure.Interfaces;

namespace PodcastReader.Phone8.ViewModels
{
    public class AddSubscriptionViewModel : RoutableViewModelBase
    {
        public AddSubscriptionViewModel()
        {
            AddSubscriptionCommand = new AddCommand();
        }

        public ICommand AddSubscriptionCommand { get; private set; }
    }

    public class AddCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            var url = (string) parameter;
            return !string.IsNullOrWhiteSpace(url);
        }

        public async void Execute(object parameter)
        {
            await RxApp.DependencyResolver.GetService<ISubscriptionsManager>().AddSubscriptionAsync();
        }

        public event EventHandler CanExecuteChanged;
    }
}