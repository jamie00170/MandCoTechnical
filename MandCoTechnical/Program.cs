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

    public class Program
    {       

        /// <summary>
        /// Returns the intended filename given the current date/time
        /// </summary>
        /// <returns></returns>
        public static string getCurrentFilename()
        {
            // get current date time
            string dateNow = DateTime.Now.ToString("yyyyMMddHH");

            string year = dateNow.Substring(0, 4);
            string month = dateNow.Substring(4, 2);
            string day = dateNow.Substring(6, 2);
            string hour = dateNow.Substring(8, 2);

            // Create the filename with the current date time information
            string filename = year + "-" + month + "-" + day + "-" + hour + ".json";           
            return filename;
        }

        /// <summary>
        /// Returns an int version of the current hour
        /// </summary>
        /// <returns></returns>
        public static int getCurrentHourInt()
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
        public static string getFilenameForHour(String currentFilename, String currentHour)
        {
            StringBuilder sb = new StringBuilder(currentFilename);
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            return sb.ToString(); ;

        }
      

        static void Main(string[] args)
        {
            XmlTextReader rssReader;
            XmlDocument rssDoc;

            rssReader = new XmlTextReader("http://feeds.bbci.co.uk/news/uk/rss.xml");

            rssDoc = new XmlDocument();
            rssDoc.Load(rssReader);


            RssFeed rssFeed = new RssFeed();

            rssFeed.ParseRssItems(rssDoc);

            //displayRssItems();       

            string filename = getCurrentFilename();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            // if Hour != 00
            if (filename.Substring(11, 2) != "00")
            {

                int currentHour = getCurrentHourInt();
                currentHour--; // don't look at file for current hour 
                while (currentHour >= 0)
                {
                    string strHour = currentHour.ToString("D2");
                    string previousFilename = getFilenameForHour(filename, strHour);
                    if (File.Exists("feed/" + previousFilename))
                    {
                        Console.WriteLine("Previous file name :" + previousFilename);
                        string newJsonSTRING = File.ReadAllText("feed/" + previousFilename);
                        RssFeed previousRssFeed = JsonConvert.DeserializeObject<RssFeed>(newJsonSTRING);

                        // Remove headlines already stored from new rss items
                        rssFeed.removeDuplicateHeadlines(previousRssFeed.Items);
                    }
                    else
                    {
                        Console.WriteLine("No file for: " + previousFilename.Substring(0, 13) + " exists!");
                    }
                    currentHour--;
                }

                // write all items still in newRssItems to file
                string newHeadlines = JsonConvert.SerializeObject(rssFeed);
                File.WriteAllText("feed/" + filename, newHeadlines);

            }
            else
            {
                // Store all headlines since it is a new day 
                Console.WriteLine("Hour is 00 ...... Resetting Headlines!");
                string AllHeadlines = JsonConvert.SerializeObject(rssFeed);
                File.WriteAllText("feed/" + filename, AllHeadlines);

            }

        }



    }
}

