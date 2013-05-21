using PodcastReader.Infrastructure.Interfaces;
using ReactiveUI;

namespace PodcastReader.Phone8.Infrastructure
{
    public static class PlayerClient
    {
        private static readonly IPlayerClient _instance = RxApp.GetService<IPlayerClient>();
        public static IPlayerClient Default { get { return _instance; } }
    }
}
