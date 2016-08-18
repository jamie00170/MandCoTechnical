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
            // get the actual current hour returned
            int currentHour = MandCoTechnical.Program.getCurrentHourInt();

            // Calculate the expected cuurent hour
            string dateNow = DateTime.Now.ToString("HH");
            int expectedHour = 0;
            Int32.TryParse(dateNow, out expectedHour);

            // Assert actual == expected
            Assert.AreEqual(currentHour, expectedHour);

        }

        [TestMethod]
        public void getFilenameForHourTest()
        {
            // get the actual filename returned
            string filenameForHour = MandCoTechnical.Program.getFilenameForHour(MandCoTechnical.Program.getCurrentFilename(), "11");

            // Calculate the expected filename
            string expectedFilename = MandCoTechnical.Program.getCurrentFilename();
            StringBuilder sb = new StringBuilder(expectedFilename);
            string currentHour = "11";
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            // Assert actual filename == expected
            Assert.AreEqual(filenameForHour, sb.ToString());

            // get the actual filename returned
            filenameForHour = MandCoTechnical.Program.getFilenameForHour(MandCoTechnical.Program.getCurrentFilename(), "23");
            
            // Calculate the expected filename
            expectedFilename = MandCoTechnical.Program.getCurrentFilename();           
            currentHour = "23";
            sb[11] = currentHour[0];
            sb[12] = currentHour[1];

            // Assert actual filename == expected
            Assert.AreEqual(filenameForHour, sb.ToString());
        }    

    }
}
