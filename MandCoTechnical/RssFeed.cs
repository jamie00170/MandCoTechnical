using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.ObjectModel;

namespace MandCoTechnical
{
    public class RssFeed
    {

        private string title;
        private string link;
        private string description;
        private Collection<RssItem> items = new Collection<RssItem>();

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Link
        {
            get { return this.link; }
            set { this.link = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public Collection<RssItem> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }

        
        private void ParseDocElements(XmlNode parent, string xPath, ref string property)
        {
            // Select the node correspinding to xPath
            XmlNode node = parent.SelectSingleNode(xPath);

            if (node != null)
                // set the property of the item to the node's inner text
                property = node.InnerText;
            else
                property = "Unresolvable";
        }

        /// <summary>
        /// Parses an rss document and extracts the title, link, description and items and stores them in an RssFeed object
        /// </summary>
        /// <param name="xmlDoc">Contains an xml document extracted from an rss feed</param>
        public void ParseRssItems(XmlDocument xmlDoc)
        {
            items.Clear();

            XmlNode title = xmlDoc.SelectSingleNode("rss/channel/title");
            XmlNode link = xmlDoc.SelectSingleNode("rss/channel/link");
            XmlNode description = xmlDoc.SelectSingleNode("rss/channel/description");

            this.title = title.InnerText;
            this.link = link.InnerText;
            this.description = description.InnerText;

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
                ParseDocElements(node, "pubDate", ref item.date);

                // Add the created item to Items
                items.Add(item);
            }

        }

        /// <summary>
        /// Used to remove items already stored in a previous file
        /// </summary>
        /// <param name="previousRssItems">Contains a collection of RssItems from a file that was generated previously during the day</param>
        public void removeDuplicateHeadlines(Collection<RssItem> previousRssItems)
        {

            RssFeed rssFeed = new RssFeed();

            Collection<RssItem> items_to_remove = new Collection<RssItem>();
            // For each item in the previous files items 
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

    }
}
