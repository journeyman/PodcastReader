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
        [Fact]
        public void CachingState_VM_InitialState_Test()
        {
            var progress = new DeferredReactiveProgress(default(ProgressValue));
            var vm = new CachingStateVm(progress);
            
            Assert.Equal(0UL, vm.CachedSize);
            Assert.Equal(0UL, vm.FinalSize);
            Assert.False(vm.IsFullyCached);
        }

        [Fact]
        public void CachingState_VM_Progressing_Test_With_Deferred_Progress()
        {
            var progress = new DeferredReactiveProgress(default(ProgressValue));
            var vm = new CachingStateVm(progress);
            
            var reporter = new OngoingReactiveProgress1();
            reporter.Report(new ProgressValue(1,10));
            progress.SetRealReactiveProgress(reporter);

            Assert.Equal(1ul, vm.CachedSize);
            Assert.Equal(10ul, vm.FinalSize);
            Assert.False(vm.IsFullyCached);
        }

        [Fact]
        public void CachingState_VM_Progressing_Test_With_Preinited_Progress()
        {
            var progress = new DeferredReactiveProgress(default(ProgressValue));
            var reporter = new OngoingReactiveProgress1();
            progress.SetRealReactiveProgress(reporter);
            reporter.Report(new ProgressValue(1, 10));

            var vm = new CachingStateVm(progress);


            Assert.Equal(1ul, vm.CachedSize);
            Assert.Equal(10ul, vm.FinalSize);
            Assert.False(vm.IsFullyCached);
        }

        //[Fact]
        public void Test_Caching_State_Initialization()
        {
            var downloaderMock = new Mock<IBackgroundDownloader>();
            downloaderMock.Setup(x => x.Load(It.IsAny<string>(), It.IsAny<IProgress<ProgressValue>>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.FromResult(new Uri("http://google.com")));
            var storageMock = new Mock<IPodcastsStorage>();
            storageMock.Setup(x => x.MoveFromTransferTempStorage(It.IsAny<Uri>(), It.IsAny<IPodcastItem>()))
                       .Returns(Task.FromResult(new Uri("http://google.com")));
            var podcastMock = new Mock<IPodcastItem>();
            podcastMock.Setup(x => x.Title).Returns("title");
            podcastMock.Setup(x => x.PodcastUri).Returns(new Uri("http://google.com"));

            var state = new CachingState(podcastMock.Object, downloaderMock.Object, storageMock.Object);
            state.Init();

            //Assert.NotNull(state.Progress);
            //Assert.True(state.IsFullyCached);
            //Assert.Equal(0d, state.FinalSize);
        }
    }
}