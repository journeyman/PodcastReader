using Splat;
using System;

namespace Pr.Core.Utils
{
    public static class CommonExtensions
    {
        public static TOut IfNotNull<TIn, TOut>(this TIn This, Func<TIn, TOut> selector, TOut fallback = default(TOut))
            where TIn : class
        {
            if (This == null)
                return fallback;
            return selector(This);
        }

		public static void RegisterLazySingleton<TService>(this IMutableDependencyResolver container, Func<TService> valueFactory, string contract = null)
		{
			container.RegisterLazySingleton(() => valueFactory(), typeof(TService), contract);
		}
    }
}
