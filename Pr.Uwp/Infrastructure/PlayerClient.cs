using Pr.Core.Interfaces;
using ReactiveUI;
using Splat;

namespace Pr.Phone8.Infrastructure
{
    public static class PlayerClient
    {
        private static readonly IPlayerClient _instance = Locator.Current.GetService<IPlayerClient>();
        public static IPlayerClient Default { get { return _instance; } }
    }
}
