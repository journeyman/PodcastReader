using PodcastReader.Phone8.Resources;

namespace PodcastReader.Phone8
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class Localized
    {
        private static readonly Strings _localizedResources = new Strings();

        public static Strings Strings { get { return _localizedResources; } }
    }
}