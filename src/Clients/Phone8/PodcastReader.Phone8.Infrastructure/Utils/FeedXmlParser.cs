using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PodcastReader.Infrastructure.Utils
{
    public static class FeedXmlParser
    {
        public static SyndicationFeed Parse(string xml)
        {
            //DtdProcessing = DtdProcessing.Ignore is needed for some feeds (e.g. http://www.dotnetrocks.com/feed.aspx)
            using (var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
            {
                var feed = SyndicationFeed.Load(reader);
                return feed;
            }
        }
    }
}
