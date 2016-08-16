using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.ObjectModel;

namespace MandCoTechnical
{
    class RssFeed
    {

        // Used to store the items read from the rss feed
        private static Collection<RssItem> rssItems = new Collection<RssItem>();

        public Collection<RssItem> RssItems
        {
            get { return this.RssItems; }
        }

        public void ParseDocElements(XmlNode parent, string xPath, ref string property)
        {
            // Select the node correspinding to xPath
            XmlNode node = parent.SelectSingleNode(xPath);

            if (node != null)
                // set the property of the item to the node's inner text
                property = node.InnerText;
            else
                property = "Unresolvable";
        }

        public void ParseRssItems(XmlDocument xmlDoc)
        {
            rssItems.Clear();
            // select all items from xmlDoc
            XmlNodeList nodes = xmlDoc.SelectNodes("rss/channel/item");
            // for each item in the document 
            foreach (XmlNode node in nodes)
            {
                // Create an Rssitem 
                RssItem item = new RssItem();
                ParseDocElements(node, "title", ref item.title);
                ParseDocElements(node, "description", ref item.description);
                ParseDocElements(node, "link", ref item.link);

                string date = null;
                ParseDocElements(node, "pubDate", ref date);
                DateTime.TryParse(date, out item.date);

                // Add the created item to rssItems
                rssItems.Add(item);
            }
        }

        /// <summary>
        /// Used for debugging - checks that rss feed has been added to rss items
        /// </summary>
        public void displayRssItems()
        {
            foreach (RssItem item in rssItems)
            {
                Console.WriteLine(Environment.NewLine + "Title: " + item.title);
                Console.WriteLine("Description: " + item.description);
                Console.WriteLine("Date: " + item.date);
                Console.WriteLine("Link: " + item.link);
            }
        }


    }
}
