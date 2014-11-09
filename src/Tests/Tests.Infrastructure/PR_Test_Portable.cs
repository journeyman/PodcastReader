using System;
using Akavache;
using Splat;

namespace Tests.Infrastructure
{
    public class PR_Test_Portable
    {
        public PR_Test_Portable()
        {
            ModeDetector.OverrideModeDetector(new InTestModeDetector());

            var cache = new Lazy<IBlobCache>(() => new TestBlobCache());
            Locator.CurrentMutable.Register(() => cache.Value, typeof(IBlobCache));
        }

        private class InTestModeDetector : IModeDetector
        {
            public bool? InUnitTestRunner() { return true; }
            public bool? InDesignMode() { return false; }
        }
    }
}
