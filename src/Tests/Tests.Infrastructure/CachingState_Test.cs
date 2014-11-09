using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PodcastReader.Infrastructure.Caching;
using PodcastReader.Infrastructure.Entities.Podcasts;
using PodcastReader.Infrastructure.Http;
using PodcastReader.Infrastructure.Storage;
using Xunit;

namespace Tests.Infrastructure
{
    public class CachingState_Test : PR_Test_Portable
    {
        private void setupCachingState(Action<IProgress<ProgressValue>> whenProgress)
        {
            var downloaderMock = new Mock<IBackgroundDownloader>();
            downloaderMock.Setup(x => x.Load(It.IsAny<string>(), It.IsAny<IProgress<ProgressValue>>(), It.IsAny<CancellationToken>()))
                .Returns<string, IProgress<ProgressValue>, CancellationToken>((url, p, ct) => p.)
            
        }

        [Fact]
        public void Test_Caching_State_Initialization()
        {
            var downloaderMock = new Mock<IBackgroundDownloader>();
            downloaderMock.Setup(x => x.Load(It.IsAny<string>(), It.IsAny<IProgress<ProgressValue>>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.FromResult(new Uri("http://google.com")));
            var storageMock = new Mock<IPodcastsStorage>();
            storageMock.Setup(x => x.CopyFromTransferTempStorage(It.IsAny<Uri>(), It.IsAny<IPodcastItem>()))
                       .Returns(Task.FromResult(new Uri("http://google.com")));
            var podcastMock = new Mock<IPodcastItem>();
            podcastMock.Setup(x => x.Title).Returns("title");
            podcastMock.Setup(x => x.PodcastUri).Returns(new Uri("http://google.com"));

            var state = new CachingState(podcastMock.Object, downloaderMock.Object, storageMock.Object);
            state.Init();

            Assert.NotNull(state.Progress);
            Assert.True(state.IsFullyCached);
            Assert.Equal(0d, state.FinalSize);
        }
    }
}