using System.IO;
using System.Xml;
using Windows.Data.Xml.Dom;
using Windows.Web.Syndication;

namespace Pr.Phone8.Infrastructure.Utils
{
    public static class FeedXmlParser
    {
        public static SyndicationFeed Parse(string xml)
        {
            //DtdProcessing = DtdProcessing.Ignore is needed for some feeds (e.g. http://www.dotnetrocks.com/feed.aspx)
	        var feed = new SyndicationFeed();
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			feed.Load(xmlDoc.GetXml());
			//feed.Load(reader.ReadContentAsString());
			return feed;
        }
    }
}
