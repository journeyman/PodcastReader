using System;

namespace Pr.Core.Caching
{
    public interface IReactiveProgress<out T> : IObservable<T>
    {
        T FinalState { get; }
    }
}