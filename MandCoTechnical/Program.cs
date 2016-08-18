using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MandCoTechnical
{

    class Program
    {

        public static Collection<RssItem> newItems = new Collection<RssItem>();

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

        /// <summary>
        /// Returns an int version of the current hour
        /// </summary>
        /// <returns></returns>
        private static int getCurrentHourInt()
        {
            string dateNow = DateTime.Now.ToString("HH");
            int currentHour = 0;
            Int32.TryParse(dateNow, out currentHour);

            return currentHour;
        }

        /// <summary>
        /// Returns the filename for the an hour given the current hour and current filename 
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="currentHour"></param>
        /// <returns></returns>
        private static string getFilenameForHour(String currentFilename, String currentHour)
        {
            StringBuilder sb = new StringBuilder(currentFilename);
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            return sb.ToString(); ;

        }

        private static Collection<RssItem> removeDuplicateHeadlines(Collection<RssItem> previousRssItems)
        {

            RssFeed rssFeed = new RssFeed();


            Collection<RssItem> items_to_remove = new Collection<RssItem>();
            foreach (RssItem item in previousRssItems)
            {
                // if item.title is already a title in rssItems remove from new RssItems
                foreach (RssItem new_item in rssFeed.rssItems)
                {
                    if (new_item.title.Equals(item.title))
                    {
                             
                        items_to_remove.Add(new_item);
                    }
                }
            }
            foreach (RssItem item in items_to_remove)
            {
                
                rssFeed.rssItems.Remove(item);
            }         

            return newItems = rssFeed.rssItems;

        }


        static void Main(string[] args)
        {
            // Create a new XML Reader
            XmlTextReader rssReader = new XmlTextReader("http://feeds.bbci.co.uk/news/uk/rss.xml");

            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(rssReader);

            RssFeed rssFeed = new RssFeed();
            rssFeed.ParseRssItems(rssDoc);


            // calculate the current filename
            String filename = getCurrentfilename();

            JavaScriptSerializer ser = new JavaScriptSerializer();          
            

            // if Hour != 00
            if (filename.Substring(11, 2) != "00")
            {
                // Check all files that already exist for that day
                int currentHour = getCurrentHourInt();
                currentHour--; // don't look at file for current hour 
                while (currentHour >= 0)
                {
                    // get the previous hours file
                    string strHour = currentHour.ToString("D2");
                    string currentFilename = getFilenameForHour(filename, strHour);
                    
                    // if a file exists for current hour
                    if (File.Exists("feed/" + currentFilename))
                    {
                        // Store the items in the file in currentRssItems
                        Console.WriteLine("Previous file name :" + currentFilename);
                        string newJsonSTRING = File.ReadAllText("feed/" + currentFilename);
                        Collection<RssItem> currentRssItems = JsonConvert.DeserializeObject<Collection<RssItem>>(newJsonSTRING);

                        // Remove headlines already stored from new rss items
                        newItems = removeDuplicateHeadlines(currentRssItems);
                       
                    }
                    else {
                        Console.WriteLine("Warning file does not exist for: " + currentFilename.Substring(0, 13));

                    }
                    // Move onto the previous file
                    currentHour--;
                }

                

                // write all items still in RssItems to file
                string newHeadlines = JsonConvert.SerializeObject(newItems);
                File.WriteAllText("feed/" + filename, newHeadlines);


            }
            else {
                // New day so write all headlines to file
                Console.WriteLine("Hour is 00 ...... Resetting Headlines!");
                string AllHeadlines = JsonConvert.SerializeObject(rssFeed.rssItems);
                File.WriteAllText("feed/" + filename, AllHeadlines);


            }


        }
        }
}
