using Splat;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

		public static async Task<string> ReadAsStringAsync(this HttpContent content, Encoding encoding)
		{
			using (var reader = new StreamReader((await content.ReadAsStreamAsync()), encoding))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
