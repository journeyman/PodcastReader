using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PodcastReader.Infrastructure.Models.Loaders;
using PodcastReader.Phone8.ViewModels;

namespace Tests.Phone8
{
    [TestClass]
    public class MainViewModelTests : PR_Test
    {
        [TestMethod]
        public void Test()
        {
            var loader = new Mock<IFeedPreviewsLoader>();
            new MainViewModel(loader.Object);
        }
    }
}