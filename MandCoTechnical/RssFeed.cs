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

        private string title;
        private string link;
        private string description;
        private Collection<RssItem> items = new Collection<RssItem>();

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
            items.Clear();
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
                items.Add(item);
            }
        }


        private void removeDuplicateHeadlines(Collection<RssItem> previousRssItems)
        {

            RssFeed rssFeed = new RssFeed();

            Collection<RssItem> items_to_remove = new Collection<RssItem>();
            foreach (RssItem item in previousRssItems)
            {
                // if item.title is already a title in rssItems remove from new RssItems
                foreach (RssItem new_item in this.items)
                {
                    if (new_item.title.Equals(item.title))
                    {

                        items_to_remove.Add(new_item);
                    }
                }
            }
            foreach (RssItem item in items_to_remove)
            {

                items.Remove(item);
            }          

        }


        /// <summary>
        /// Used for debugging - checks that rss feed has been added to rss items
        /// </summary>
        public void displayRssItems()
        {
            foreach (RssItem item in items)
            {
                Console.WriteLine(Environment.NewLine + "Title: " + item.title);
                Console.WriteLine("Description: " + item.description);
                Console.WriteLine("Date: " + item.date);
                Console.WriteLine("Link: " + item.link);
            }
        }


    }
}
