using PodcastReader.Infrastructure.Utils;
using Xunit;

namespace Tests.Infrastructure
{
    public class SomePodcastsUtilityMethodsTests
    {
        [Fact]
        public void Slugnaming_Logic_Test()
        {
            var slug = "Hobby Talks #78 - Растафарианство".ToSlug();

            Assert.Equal("Hobby-Talks-#78-Растафарианство", slug);
        }
    }
}
