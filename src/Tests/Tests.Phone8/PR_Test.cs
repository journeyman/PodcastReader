using Moq;
using Splat;

namespace Tests.Phone8
{
    /// <summary>
    /// Base class for tests in PodcastReader
    /// </summary>
    public class PR_Test
    {
        public PR_Test()
        {
            var detector = new Mock<IModeDetector>();
            detector.Setup(x => x.InUnitTestRunner()).Returns(true);

            ModeDetector.OverrideModeDetector(detector.Object);
        }
    }
}