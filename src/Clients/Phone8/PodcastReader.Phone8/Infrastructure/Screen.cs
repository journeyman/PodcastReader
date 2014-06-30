using ReactiveUI;
using Splat;

namespace PodcastReader.Phone8.Infrastructure
{
    public static class Screen
    {
        private static readonly IScreen _instance = Locator.Current.GetService<IScreen>();

        public static IScreen Instance { get { return _instance; } }
        public static RoutingState Router { get { return _instance.Router; } }
    }
}
