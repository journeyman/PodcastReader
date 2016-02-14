using Akavache;
using Splat;

namespace Pr.Core
{
    public static class Cache
    {
	    public static IBlobCache Local { get; } = Locator.Current.GetService<IBlobCache>();
    }
}
