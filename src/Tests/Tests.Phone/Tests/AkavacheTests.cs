using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Akavache;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PodcastReader.Infrastructure.Entities.Feeds;
using PodcastReader.Infrastructure.Models.Loaders;
using PodcastReader.Phone8.Infrastructure;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;

namespace Tests.Phone
{
    [TestClass]
    public class AkavacheTests : PR_Test
    {
        [TestMethod]
        public void Akavache_Test()
        {
            BlobCache.ApplicationName = "Test";

            var doesntThrow = BlobCache.InMemory.GetObject<string>("unknown key").FirstOrDefaultAsync().Wait();
        }
    }
}
