using System;
using System.Diagnostics;
using Microsoft.Phone.Reactive;
using PodcastReader.Infrastructure.Http;

namespace PodcastReader.Infrastructure.Caching
{
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
            Debug.Assert(FinalState != default(ulong) && FinalState == (ulong)value.Total, "Total value has been changed");
            FinalState = (ulong)value.Total;
            _reporter.OnNext((ulong)value.Current);
        }
    }
}