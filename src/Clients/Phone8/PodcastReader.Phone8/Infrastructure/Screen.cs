using ReactiveUI;

namespace PodcastReader.Phone8.Infrastructure
{
    public static class Screen
    {
        private static readonly IScreen _instance = RxApp.DependencyResolver.GetService<IScreen>();

        public static IScreen Instance { get { return _instance; } }
        public static IRoutingState Router { get { return _instance.Router; } }
    }
}
