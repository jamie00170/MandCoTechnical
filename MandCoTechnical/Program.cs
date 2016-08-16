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
        /// <summary>
        /// Returns the intended filename given the current date/time
        /// </summary>
        /// <returns></returns>
        private static string getCurrentfilename()
        {
            // get current date time
            string dateNow = DateTime.Now.ToString("yyyyMMddHH");

            string year = dateNow.Substring(0, 4);
            string month = dateNow.Substring(4, 2);
            string day = dateNow.Substring(6, 2);
            string hour = dateNow.Substring(8, 2);

            // Create the filename with the current date time information
            string filename = year + "-" + month + "-" + day + "-" + hour + ".json";
            Console.WriteLine("filename: " + filename);
            return filename;
        }



        static void Main(string[] args)
        {
            // Create a new XML Reader
            XmlTextReader rssReader = new XmlTextReader("http://feeds.bbci.co.uk/news/uk/rss.xml");

            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(rssReader);

            RssFeed rssFeed = new RssFeed();
            rssFeed.ParseRssItems(rssDoc);

            String filename = getCurrentfilename();      


        }
    }
}
