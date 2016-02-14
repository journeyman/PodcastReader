using ReactiveUI;
using Splat;

namespace Pr.Phone8.Infrastructure
{
    public static class Screen
    {
	    public static IScreen Instance { get; } = Locator.Current.GetService<IScreen>();

	    public static RoutingState Router => Instance.Router;
    }
}
