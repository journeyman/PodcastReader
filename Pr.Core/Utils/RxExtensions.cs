using System;
using System.Reactive.Linq;

namespace Pr.Core.Utils
{
    public static class RxExtensions
    {
		public static IObservable<T> SkipOnException<T>(this IObservable<T> source)
		{
			return source
					.Select(t => Observable.Start(() => t).Catch<T, Exception>(x => Observable.Empty<T>()))
					.Merge();
		}

		public static IObservable<R> SelectAndSkipOnException<T, R>(this IObservable<T> source, Func<T, R> selector)
        {
            return source
                    .Select(t => Observable.Start(() => selector(t)).Catch<R, Exception>(x => Observable.Empty<R>()))
                    .Merge();
        }

        public static IObservable<R> SelectManyAndSkipOnException<T, R>(this IObservable<T> source, Func<T, IObservable<R>> selector)
        {
            return source
                    .SelectMany(t => Observable.Start(() => selector(t).Catch<R, Exception>(x => Observable.Empty<R>())))
                    .Merge();
        }
    }
}