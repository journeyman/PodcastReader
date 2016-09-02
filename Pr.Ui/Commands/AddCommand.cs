using System;
using Pr.Core.Interfaces;

namespace Pr.Ui.Commands
{
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
}