using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FETP
{
    /**************************************************************************\
    Class: FETP_Controller (Final Exam Timetabling Problem Controller)
    Description: This class contains all of the primary functions used for 
    reading in data, adjusting data, outputting data, and running starting the process of running the genetic algorithm. This is the primary interface between the front-end and the back-end. It reads from the data file, formats the data for use by the GA_Controller, receives the solutions from the GA_Controller, and sends them back to the front-end for display in the GUI.
    \**************************************************************************/
    class FETP_Controller
    {
        /**************************************************************************\
        FETP_Controller - Data Members
        \**************************************************************************/
        List<string> Meeting_Days_Times;
        List<string> SUM_ACTUAL_ENROLLMENT;


        /**************************************************************************\
        FETP_Controller - Methods 
        \**************************************************************************/
        public FETP_Controller()
        {
        }

        public void readInputFile()
        {
            Meeting_Days_Times = new List<string>();
            SUM_ACTUAL_ENROLLMENT = new List<string>();

            var reader = new StreamReader(File.OpenRead(@"Spring 2015 Total Enrollments by Meeting times.csv"));

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                Meeting_Days_Times.Add(values[0]);
                SUM_ACTUAL_ENROLLMENT.Add(values[1]);
            }
        }

    }
}
