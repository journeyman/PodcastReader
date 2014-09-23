using System;

namespace PodcastReader.Infrastructure.Caching
{
    public interface IReactiveProgress<out T> : IObservable<T>
    {
        T FinalState { get; }
    }
}