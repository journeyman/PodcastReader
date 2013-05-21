using Microsoft.VisualStudio.TestTools.UnitTesting;
using PodcastReader.Infrastructure.Utils;

namespace Tests.Infrastructure
{
    [TestClass]
    public class PodcastDetectingTest
    {
        [TestMethod]
        public void MediaTypes_Pattern_Mathing_Works()
        {
            var values = new[] {"mp3", "wav"};
            var containsMP3 = "audio/mp3".ContainsValues(values);
            var containsWAV = "audio/wav1asdf".ContainsValues(values);

            Assert.IsTrue(containsMP3);
            Assert.IsTrue(containsWAV);
        }
    }
}
