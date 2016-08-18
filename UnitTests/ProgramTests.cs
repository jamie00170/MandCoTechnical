using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MandCoTechnical;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void getCurrentFilenameTest()
        {   
            // Get actual filename given        
            string currentFilename = MandCoTechnical.Program.getCurrentFilename();

            // Calculate expected filename
            string dateNow = DateTime.Now.ToString("yyyyMMddHH");
            string expectedFilename = dateNow.Substring(0, 4) + "-" + dateNow.Substring(4, 2) + "-" + 
                                      dateNow.Substring(6, 2) + "-" + dateNow.Substring(8, 2) + ".json";

            // Asset Actual == Expcted
            Assert.AreEqual(currentFilename, expectedFilename);
        }

        [TestMethod]
        public void getCurrentHourIntTest()
        {
            int currentHour = MandCoTechnical.Program.getCurrentHourInt();

            string dateNow = DateTime.Now.ToString("HH");
            int expectedHour = 0;
            Int32.TryParse(dateNow, out expectedHour);

            Assert.AreEqual(currentHour, expectedHour);

        }

        [TestMethod]
        public void getFilenameForHourTest()
        {
            string filenameForHour = MandCoTechnical.Program.getFilenameForHour(MandCoTechnical.Program.getCurrentFilename(), "11");

            string expectedFilename = MandCoTechnical.Program.getCurrentFilename();

            StringBuilder sb = new StringBuilder(expectedFilename);

            string currentHour = "11";
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            Assert.AreEqual(filenameForHour, sb.ToString());


            filenameForHour = MandCoTechnical.Program.getFilenameForHour(MandCoTechnical.Program.getCurrentFilename(), "23");

            expectedFilename = MandCoTechnical.Program.getCurrentFilename();

            currentHour = "23";
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            Assert.AreEqual(filenameForHour, sb.ToString());
        }


        [TestMethod]
        public void parseRssItemsTest()
        {
            // read up test xml file

            // create RssFeed object
           

            // aserrt title, link description and items are as expected


        }


    }
}
