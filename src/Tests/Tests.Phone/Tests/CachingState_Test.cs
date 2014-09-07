using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using PodcastReader.Infrastructure.Caching;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;
using Splat;

namespace Tests.Phone.Tests
{
    public class InTestModeDetector : IModeDetector
    {
        public bool? InUnitTestRunner() { return true; }
        public bool? InDesignMode() { return false; }
    }

    [TestClass]
    public class CachingState_Test
    {
        public CachingState_Test()
        {
            ModeDetector.OverrideModeDetector(new InTestModeDetector());
        }

        [TestMethod]
        public void Test()
        {
            var downloaderMock = new Mock<IBackgroundDownloader>();
            downloaderMock.Setup(
                x => x.Load(It.IsAny<string>(), It.IsAny<IProgress<ProgressValue>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Uri("http://google.com")));
            var storageMock = new Mock<IPodcastsStorage>();
            storageMock.Setup(x => x.CopyFromTransferTempStorage(It.IsAny<Uri>(), It.IsAny<IPodcastItem>())).Returns(Task.FromResult(new Uri("http://google.com")));
            var podcastMock = new Mock<IPodcastItem>();
            podcastMock.Setup(x => x.Title).Returns("title");

            var state = new CachingState(podcastMock.Object, downloaderMock.Object, storageMock.Object);
        }
    }
}