using PodcastReader.Infrastructure.Utils;
using Xunit;

namespace Tests.Infrastructure
{
    public class PodcastDetectingTest
    {
        [Fact]
        public void MediaTypes_Pattern_Mathing_Works()
        {
            var values = new[] {"mp3", "wav"};
            var containsMP3 = "audio/mp3".ContainsValues(values);
            var containsWAV = "audio/wav1asdf".ContainsValues(values);

            Assert.True(containsMP3);
            Assert.True(containsWAV);
        }
    }
}
