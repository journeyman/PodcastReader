using ReactiveUI;
using ReactiveUI.Routing;

namespace PodcastReader.Phone8.Infrastructure
{
    public static class Screen
    {
        private static readonly IScreen _instance = RxApp.GetService<IScreen>();

        public static IRoutingState Router { get { return _instance.Router; } }
    }
}
