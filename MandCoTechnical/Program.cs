using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MandCoTechnical
{
    class Program
    {

        static void Main(string[] args)
        {
            // Create a new XML Reader
            XmlTextReader rssReader = new XmlTextReader("http://feeds.bbci.co.uk/news/uk/rss.xml");

            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(rssReader);

            RssFeed rssFeed = new RssFeed();

            rssFeed.ParseRssItems(rssDoc);

            rssFeed.displayRssItems();       


        }
    }
}
