using ReactiveUI;

namespace PodcastReader.Phone8.Infrastructure
{
    public static class Screen
    {
        private static readonly IScreen _instance = RxApp.MutableResolver.GetService<IScreen>();

        public static IRoutingState Router { get { return _instance.Router; } }
    }
}
