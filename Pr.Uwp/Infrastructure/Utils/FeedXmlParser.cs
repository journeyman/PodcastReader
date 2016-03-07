using System.IO;
using System.Xml;
using Windows.Web.Syndication;

namespace Pr.Phone8.Infrastructure.Utils
{
    public static class FeedXmlParser
    {
        public static SyndicationFeed Parse(string xml)
        {
            //DtdProcessing = DtdProcessing.Ignore is needed for some feeds (e.g. http://www.dotnetrocks.com/feed.aspx)
            using (var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
            {
	            var feed = new SyndicationFeed();
				feed.Load(reader.ReadContentAsString());
				return feed;
            }
        }
    }
}
