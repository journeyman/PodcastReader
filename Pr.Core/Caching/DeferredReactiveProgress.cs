using System;
using System.Reactive.Subjects;
using Pr.Core.Http;

namespace Pr.Core.Caching
{
    public class DeferredReactiveProgress : IReactiveProgress<ProgressValue>
    {
        private readonly ISubject<ProgressValue> _subject;

        public DeferredReactiveProgress(ProgressValue initial)
        {
            _subject = new BehaviorSubject<ProgressValue>(initial);
        }

        public void SetRealReactiveProgress(IReactiveProgress<ProgressValue> real)
        {
            this.FinalState = real.FinalState;
            real.Subscribe(_subject);
        }

        public IDisposable Subscribe(IObserver<ProgressValue> observer)
        {
            return _subject.Subscribe(observer);
        }

        public ProgressValue FinalState { get; private set; }
    }
}