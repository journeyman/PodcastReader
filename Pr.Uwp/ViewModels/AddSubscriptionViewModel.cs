using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using System.Windows.Input;
using Pr.Core.Interfaces;
using Pr.Phone8.Infrastructure.Commands;

namespace Pr.Phone8.ViewModels
{
    public class AddSubscriptionViewModel : RoutableViewModelBase
    {
        public AddSubscriptionViewModel(ISubscriptionsManager subscriptionsManager)
        {
            AddSubscriptionCommand = new AddCommand(subscriptionsManager);
        }

        public ICommand AddSubscriptionCommand { get; private set; }
    }

    public class AddCommand : CommandBase<string>
    {
        private readonly ISubscriptionsManager _subscriptionsManager;

        public AddCommand(ISubscriptionsManager subscriptionsManager)
        {
            _subscriptionsManager = subscriptionsManager;
        }

        protected override bool CanExecute(string param)
        {
            return !string.IsNullOrWhiteSpace(param);
        }

        protected override void Execute(string param)
        {
            _subscriptionsManager.AddSubscriptionAsync(new Subscription(new Uri(param)));
        }
    }

    public class Subscription : ISubscription
    {
        private readonly Uri _uri;

        public Subscription(Uri uri) { _uri = uri; }

        public Uri Uri { get { return _uri; } }
    }
}