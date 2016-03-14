using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;  // allows times to be different pased on local

/*

    Ben Notes:
        swaped enrollment and days inorder for the call to the class constructor from block cleaner

        do we really want to make block a class?
            it gets weird with construction
            its not REALLY a class
            it is probably for the best

        
            could change and give it property isMWF to determine days
            days of blocks SHOULD only be MWF or TR

        timeslot class
            doesn't seem to be needed
            only thing to think to do is limit number of slots, but thats not needed

*/



namespace FETP
{
    // Programmer: Ben and Vic
    public class Class
    {
        
        protected TimeSpan startTime;
        protected TimeSpan endTime;
        protected int enrollment;
        protected List<DayOfWeek> daysMeet;
        
        // Accessors and Mutators
        public TimeSpan StartTime 
        {
            get { return this. startTime; }
            set { this.startTime = value; }
        }
        
        public TimeSpan EndTime 
        {
            get { return this.endTime; }
            set { this.startTime = value; }
        }
        
        public int Enrollment
        {
            get { return this.enrollment; }
            set { this.enrollment = value; }
        }

        public List<DayOfWeek> DaysMeet
        {
            get { return this.daysMeet; }
            set { this.daysMeet = value;  }
        }
        
        
        // Intializes Class
        public Class(TimeSpan inStartTime, TimeSpan inEndTime, List<DayOfWeek> inDaysMeet = null, int inEnrollment = 0)
        {
            this.startTime = inStartTime;
            this.endTime = inEndTime;
            this.enrollment = inEnrollment;
            this.daysMeet = inDaysMeet;
        }

        public void Display()
        {
            Console.WriteLine("Start Time: {0}", this.startTime);
            Console.WriteLine("End Time: {0}", this.endTime);
            Console.WriteLine("Enrollment: {0}", this.enrollment);
            Console.Write("Days Meet: ");
            foreach (DayOfWeek day in daysMeet)
            {
                Console.Write("{0} ", day);
            }
            
            Console.WriteLine("");
        }

    }

    // block contains classes that overlapped and were coalesced
    public class Block : Class
    {
        protected List<Class> classesInBlock;

        public Block(TimeSpan inStartTime, TimeSpan inEndTime, List<DayOfWeek> inDaysMeet, List<Class> inClasses)
            : base(inStartTime, inEndTime, inDaysMeet)
        {
            this.classesInBlock = inClasses;

            // calculate total enrollement
            foreach (Class clas in inClasses)
                this.enrollment += clas.Enrollment;

        }

        
        public void DisplayAllClasses()
        {
            foreach (Class cl in classesInBlock)
                cl.Display();
        }
        
    }

    // playing with TimeSlot class
    //hmmmm
    public class TimeSlots
    {
        protected List<Block> timeSlots;

        public TimeSlots(List<Block> inBlocks)
        {
            this.timeSlots = inBlocks;
        }

        public void Display()
        {
            foreach (Block block in timeSlots)
                block.Display();
        }
    }

    public class Schedule
    {
        protected List<TimeSlots> days;

        public Schedule(List<TimeSlots> inDays)
        {
            this.days = inDays;
        }
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
    public static class FETP_Controller
    {
        // These are the values that determine the boundaries of classes to be ignored
        private const string CLASS_LENGTH_TO_START_IGNORING = "02:45";
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "18:00";

        // Programmer: Ben
        // takes in an open data file and returns a list of all the classes
        public static List<Class> readInputDataFile(FileStream inFile)
        {

            // Make boundaries of ignored classes more usable
            TimeSpan ignoreClassLength = TimeSpan.ParseExact(CLASS_LENGTH_TO_START_IGNORING, @"hh\:mm", CultureInfo.InvariantCulture);
            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(HOUR_TO_BEGIN_IGNORE_CLASS, @"hh\:mm", CultureInfo.InvariantCulture);

            List<Class> allClasses = new List<Class>(); // list of all classes to be returned

            var reader = new StreamReader(inFile);

            reader.ReadLine(); // skip description line
        
            while (!reader.EndOfStream)
            {
                // ? possibly change var to string
                var line = reader.ReadLine(); // reads in next line
                var values = line.Split(','); // splits into days/times and enrollement
                var daysAndTimes = values[0].Split(' '); // chops up the days and times to manageable sections



                TimeSpan startTime = TimeSpan.ParseExact(daysAndTimes[1], @"hhmm", CultureInfo.InvariantCulture); // 1 postion is the start time, changes formated time to bw more usable 
                TimeSpan endTime = TimeSpan.ParseExact(daysAndTimes[3], @"hhmm", CultureInfo.InvariantCulture); // 3 position is the end time, changes formated time to bw more usable 

                // Checks if class should not be ignored before continuing execution
                if ((startTime < ignoreClassStartTime) || (endTime - startTime < ignoreClassLength))
                {
                    List<DayOfWeek> days = new List<DayOfWeek>(); // days the class meets
                    foreach (char day in daysAndTimes[0].ToCharArray()) // changes days from string of chars to list of DayOfWeek type
                    {
                        switch (day)
                        {
                            case 'M':
                                days.Add(DayOfWeek.Monday);
                                break;
                            case 'T':
                                days.Add(DayOfWeek.Tuesday);
                                break;
                            case 'W':
                                days.Add(DayOfWeek.Wednesday);
                                break;
                            case 'R':
                                days.Add(DayOfWeek.Thursday);
                                break;
                            case 'F':
                                days.Add(DayOfWeek.Friday);
                                break;
                        }
                    }

                    int enrollment = Int32.Parse(values[1]); // enrollement values should be in 1 position

                    allClasses.Add(new Class(startTime, endTime, days, enrollment)); // add new Class to list

                }
            }

            return allClasses;

        }

        // Checks if two classes overlap
        public static bool doClassesOverlap(Class class1, Class class2)
        {
            // Broke up to aid readability
            return (
               (getNumberOfDaysInCommon(class1, class2) > 0) // do the classes have any days in common
               && (((class1.StartTime >= class2.StartTime && class1.StartTime <= class2.EndTime) // does class1 start during class2
               || (class1.EndTime >= class2.StartTime && class1.EndTime <= class2.EndTime)) // does class1 end during class2 
               || ((class2.StartTime >= class1.StartTime && class2.StartTime <= class1.EndTime) // does class2 start during class1
               || (class2.EndTime >= class1.StartTime && class2.EndTime <= class1.EndTime))) // does class2 end during class1
            );              
                
            
        }

        // Helper function to find the total days two classes have in common.
        public static int getNumberOfDaysInCommon(Class class1, Class class2)
        {
            int daysInCommon = 0;
            foreach(DayOfWeek dayFromClass1 in class1.DaysMeet)
            {
                foreach(DayOfWeek dayFromClass2 in class2.DaysMeet)
                {
                    if (dayFromClass1 == dayFromClass2) daysInCommon++;
                }
            }
            return daysInCommon;
        }

        // in theory, this will automatically find what days a block meets.
        // a block should only meet on MWF or TR
        public static List<DayOfWeek> getMostCommonDays(List<Class> classes)
        {
            return null;
        }

    }
}
