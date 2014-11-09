using Splat;

namespace Tests.Phone
{
    public class PR_Test
    {
        public PR_Test()
        {
            ModeDetector.OverrideModeDetector(new InTestModeDetector());
        }

        private class InTestModeDetector : IModeDetector
        {
            public bool? InUnitTestRunner() { return true; }
            public bool? InDesignMode() { return false; }
        }
    }
}
