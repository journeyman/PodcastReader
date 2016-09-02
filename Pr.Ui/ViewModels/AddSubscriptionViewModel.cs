using System.Windows.Input;
using Pr.Core.Interfaces;
using Pr.Ui.Commands;

namespace Pr.Ui.ViewModels
{
	public class AddSubscriptionViewModel : RoutableViewModelBase
	{
		public AddSubscriptionViewModel(ISubscriptionsManager subscriptionsManager)
		{
			AddSubscriptionCommand = new AddCommand(subscriptionsManager);
		}

		public ICommand AddSubscriptionCommand { get; private set; }
	}
}