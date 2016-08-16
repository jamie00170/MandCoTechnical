using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;

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

                    currentHour--;
                }


            } else {
                // New day so write all headlines to file
                Console.WriteLine("Hour is 00 ...... Resetting Headlines!");
                string AllHeadlines = JsonConvert.SerializeObject(rssFeed.RssItems);
                File.WriteAllText("feed/" + filename, AllHeadlines);


            }


        }
        }
}
