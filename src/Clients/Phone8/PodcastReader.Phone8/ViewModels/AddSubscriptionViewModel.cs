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
        public AddSubscriptionViewModel(ISubscriptionsManager subscriptionsManager)
        {
            AddSubscriptionCommand = new AddCommand(subscriptionsManager);
        }

        public ICommand AddSubscriptionCommand { get; private set; }
    }

    public class AddCommand : ICommand
    {
        private readonly ISubscriptionsManager _subscriptionsManager;

        public AddCommand(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;
        }

        public bool CanExecute(object parameter)
        {
            var url = (string) parameter;
            return !string.IsNullOrWhiteSpace(url);
        }

        public void Execute(object parameter)
        {
            _subscriptionsManager.AddSubscriptionAsync(new Subscription(new Uri((string)parameter)));
        }

        public event EventHandler CanExecuteChanged;
    }

    public class Subscription : ISubscription
    {
        private readonly Uri _uri;

        public Subscription(Uri uri) { _uri = uri; }

        public Uri Uri { get { return _uri; } }
    }
}