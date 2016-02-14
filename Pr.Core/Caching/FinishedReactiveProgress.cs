using System;
using System.Reactive.Linq;

namespace Pr.Core.Caching
{
    public class FinishedReactiveProgress<T> : IReactiveProgress<T>
    {
        public FinishedReactiveProgress(T finalState)
        {
            FinalState = finalState;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return Observable.Return(FinalState).StartWith(FinalState).Subscribe(observer);
        }

        public T FinalState { get; private set; }
    }
}