using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FETP
{
    struct Class
    {
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int enrollment { get; set; }
    }

    /**************************************************************************\
    Class: FETP_Controller (Final Exam Timetabling Problem Controller)
    Description: This class contains all of the primary functions used for 
    reading in data, adjusting data, outputting data, and running starting the 
    process of running the genetic algorithm. This is the primary interface 
    between the front-end and the back-end. It reads from the data file, formats 
    the data for use by the GA_Controller, receives the solutions from the 
    GA_Controller, and sends them back to the front-end for display in the GUI.
    \**************************************************************************/
    public class FETP_Controller
    {
        /**************************************************************************\
        FETP_Controller - Data Members
        \**************************************************************************/
        List<string> Meeting_Days_Times;
        List<string> SUM_ACTUAL_ENROLLMENT;

        private int numExamSlots;
        private int numClasses;
        Class[] setClasses;

        /**************************************************************************\
        FETP_Controller - Methods 
        \**************************************************************************/
        /**************************************************************************\
        Constructor: Default 
        Description: 
        \**************************************************************************/ 
        public FETP_Controller()
        {
        }


        /**************************************************************************\
        Method: ReadEnrollmentFile 
        Description:  
        \**************************************************************************/ 
        public void ReadEnrollmentFile()
        {
            numClasses = 0;
            Meeting_Days_Times = new List<string>();
            SUM_ACTUAL_ENROLLMENT = new List<string>();

            var reader = new StreamReader(File.OpenRead(@"Spring 2015 Total Enrollments by Meeting times.csv"));

            bool isDescriptionLine = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                //Skips the first data description line in the csv
                if (!isDescriptionLine)
                {
                    Meeting_Days_Times.Add(values[0]);
                    SUM_ACTUAL_ENROLLMENT.Add(values[1]);
                    numClasses++;
                }
                //Setting to false after reading the first line
                //This could use a more robust solution in case a future input file doesn't have a description
                isDescriptionLine = false;
            }
        }


        /**************************************************************************\
        Method: ParseClasses
        Description:  
        \**************************************************************************/ 
        public void ParseClasses()
        {
            setClasses = new Class[numClasses];

            for (int i = 0; i < numClasses; i++)
            {
                //Parses enrollment and adds it to class struct
                string strEnrollment = SUM_ACTUAL_ENROLLMENT[i].ToString();
                setClasses[i].enrollment = Int32.Parse(strEnrollment);

                //Converting meeting days and times to a char array for parsing
                string meetingDaysTimes = Meeting_Days_Times[i].ToString();
                Char[] charMeetingDaysTimes = meetingDaysTimes.ToCharArray();
                
                //Used for determining if class start time or end time
                bool isParsingEndTime = false;

                StringBuilder sbStartTime = new System.Text.StringBuilder();
                StringBuilder sbEndTime = new System.Text.StringBuilder();
                string strStartTime;
                string strEndTime;

                for (int j = 0; j < charMeetingDaysTimes.Length; j++)
                {
                    if (Char.IsLetter(charMeetingDaysTimes[j]))
                    {
                        //Identifies days of week class is held on and adds to class struct
                        switch (charMeetingDaysTimes[j])
                        {
                            case 'M':
                                setClasses[i].monday = true;
                                break;
                            case 'T':
                                setClasses[i].tuesday = true;
                                break;
                            case 'W':
                                setClasses[i].wednesday = true;
                                break;
                            case 'R':
                                setClasses[i].thursday = true;
                                break;
                            case 'F':
                                setClasses[i].friday = true;
                                break;
                        }
                    }
                    //Parsing class start and end times
                    else if (Char.IsDigit(charMeetingDaysTimes[j]))
                    {
                        if (!isParsingEndTime)
                        {
                            sbStartTime.Append(charMeetingDaysTimes[j]);
                        }
                        else
                        {
                            sbEndTime.Append(charMeetingDaysTimes[j]);
                        }
                    }
                    else if (charMeetingDaysTimes[j] == '-')
                    {
                        isParsingEndTime = true;
                    }
                }

                strStartTime = sbStartTime.ToString();
                strEndTime = sbEndTime.ToString();
                setClasses[i].startTime = DateTime.ParseExact(strStartTime, "HHmm", System.Globalization.CultureInfo.InvariantCulture);
                setClasses[i].endTime = DateTime.ParseExact(strEndTime, "HHmm", System.Globalization.CultureInfo.InvariantCulture);
            }

        }


        /**************************************************************************\
        Method: setExamSlots 
        Description: Sets the number of possible exam slots 
        \**************************************************************************/ 
        public void setExamSlots(int numberOfExamSlots)
        {
            numExamSlots = numberOfExamSlots;
        }

    }
}
