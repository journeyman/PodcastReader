using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using PodcastReader.Infrastructure.Http;

namespace PodcastReader.Infrastructure.Caching
{
    public class OngoingReactiveProgress1 : IReactiveProgress<ProgressValue>, IProgress<ProgressValue>
    {
        private readonly ISubject<ProgressValue> _reporter = new BehaviorSubject<ProgressValue>(default(ProgressValue));
        public IDisposable Subscribe(IObserver<ProgressValue> observer)
        {
            return _reporter.Subscribe(observer);
        }

        public ProgressValue FinalState { get; private set; }

        public void Report(ProgressValue value)
        {
            Debug.Assert(FinalState.Total != default(ulong) ^ FinalState.Current != value.Total, "Total value has been changed");
            FinalState = new ProgressValue(value.Total, value.Total);
            _reporter.OnNext(value);
        }
    }

    public class OngoingReactiveProgress : IReactiveProgress<ulong>, IProgress<ProgressValue>
    {
        private readonly ISubject<ulong> _reporter = new ReplaySubject<ulong>(); 
        public IDisposable Subscribe(IObserver<ulong> observer)
        {
            return _reporter.Subscribe(observer);
        }

        public ulong FinalState { get; private set; }
        
        public void Report(ProgressValue value)
        {
            Debug.Assert(FinalState != default(ulong) && FinalState == value.Total, "Total value has been changed");
            FinalState = value.Total;
            _reporter.OnNext(value.Current);
        }
    }
}