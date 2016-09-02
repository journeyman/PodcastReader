using ReactiveUI;
using Splat;

namespace Pr.Ui.Core.Navigation
{
	public static class Screen
	{
		public static IScreen Current { get; } = Locator.Current.GetService<IScreen>();
	}
}