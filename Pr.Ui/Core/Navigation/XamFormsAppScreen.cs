using ReactiveUI;

namespace Pr.Ui.Core.Navigation
{
	public class XamFormsAppScreen : IScreen
	{
		public RoutingState Router { get; } = new RoutingState();
	}
}
