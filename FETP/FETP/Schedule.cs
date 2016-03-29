using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // ?? some of these aren't needed

using System.Globalization;  // allows times to be different pased on local ? may can be removed
using System.Diagnostics;
using System.IO;


namespace FETP
{

    // ? if a function can modify the internal variables of class, it should be in that class
    // ? 
    /**************************************************************************\
    Class: Schedule (Genetic Algorithm Constraints)
    Description: The Constraints class contains all of the methods needed for
                 checking if a chromosome is meeting soft and hard constraints. 
    \**************************************************************************/
    public static class Schedule
    {

        /**************************************************************************\
        Schedule - Constant Data Members 
        \**************************************************************************/
        private const string CLASS_LENGTH_TO_START_IGNORING = "0126"; // ? clean these up
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        #region Static Data Members 
        /**************************************************************************\
        Schedule - Static Data Members 
        \**************************************************************************/
        private static int numberOfDays;
        private static TimeSpan examsStartTime;
        private static TimeSpan examsLength;
        private static TimeSpan timeBetweenExams;
        private static TimeSpan lunchLength;
        private static List<Class> allClasses;

        private static int numberOfTimeSlotsAvailable;
        #endregion


        /**************************************************************************\
        Schedule - Data Members 
        \**************************************************************************/
        private static Block[] blocks;
        //private List<TimeSpan> startTimes = new List<TimeSpan>();

        #region Properties
        /**************************************************************************\
        Schedule - Properties 
        \**************************************************************************/
        public static Block[] Blocks
        {
            get
            {
                return Schedule.blocks;
            }
            //set
            //{
            //    this.blocks = value;
            //}
        }
        public static int NumberOfDays
        {
            get { return numberOfDays; }
            //set { Schedule.numberOfDays = value; }
        }
        public static TimeSpan ExamsStartTime
        {
            get { return examsStartTime; }
            //set { Schedule.examsStartTime = value; }
        }
        public static TimeSpan ExamsLength
        {
            get { return examsLength; }
            //set { Schedule.examsLength = value; }
        }
        public static TimeSpan TimeBetweenExams
        {
            get { return timeBetweenExams; }
            //set { Schedule.timeBetweenExams = value; }
        }
        public static TimeSpan LunchLength
        {
            get { return lunchLength; }
            //set { this.lunchLength = value; }
        }
        public static List<Class> AllClasses
        {
            get
            {
                return Schedule.allClasses;
            }
        }
        public static int NumberOfTimeSlotsAvailable
        {
            get
            {
                return Schedule.numberOfTimeSlotsAvailable;
            }
            
        }
        public static int NumberOfTimeSlotsAvailablePerDay
        {
            get
            {
                TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go // ? maybe rewrite

                TimeSpan lengthOfExamDay = latestTime - Schedule.ExamsStartTime; // Figure out how much time available for exams

                // if the lunch time is longer than the break time, account for it and the extra break time it will give you
                if (Schedule.LunchLength > Schedule.TimeBetweenExams)
                {
                    lengthOfExamDay -= (Schedule.LunchLength - Schedule.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
                }

                TimeSpan examFootprint = Schedule.ExamsLength + Schedule.TimeBetweenExams;

                int numberOfExams = 0;
                // ? bug if exam break is too big
                while ((lengthOfExamDay - Schedule.ExamsLength) >= TimeSpan.Zero)
                {
                    lengthOfExamDay -= Schedule.ExamsLength;
                    numberOfExams++;

                    // checks if a break is needed due to their being room for another exam after a break
                    if ((lengthOfExamDay - (Schedule.TimeBetweenExams + Schedule.ExamsLength) >= TimeSpan.Zero))
                    {
                        lengthOfExamDay -= Schedule.TimeBetweenExams;
                    }
                }
                return numberOfExams;
            }
        }
        //public double FitnessScore
        //{
        //    get
        //    {
        //        double fitnessScore = 0;
        //        foreach(Block block in this.blocks)
        //        {
        //            fitnessScore += block.FitnessScore;
        //        }
        //        // more weighting stuff here
        //        return fitnessScore;
        //    }
        //} // ? possible optimation, if private var is null, set it. then return. ? maybe add in Parallel foreach loop to do that if fitness score is heavy to calculate?
        #endregion

        /**************************************************************************\
        Schedule - Methods 
        \**************************************************************************/
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        //public Schedule(List<Block> inDays = null)
        //{
        //    if (inDays == null)
        //    {
        //        this.blocks = new List<Block>();
        //    } 
        //    else
        //    {
        //        this.blocks = inDays;
        //    }
        //}

      
        public static List<Block> ScheduleBlocks(List<Block> groupedClasses)
        {
            Schedule.blocks = new Block[Schedule.NumberOfTimeSlotsAvailable];
            List<Block> sortedGroupedClasses = groupedClasses.OrderByDescending(c => c.Enrollment).ToList();
            while(sortedGroupedClasses.Count > 0 && !Schedule.IsFull())
            {

                int index = FindBestTimeslotFit(sortedGroupedClasses[0].WeightedAverageStartTime);
                blocks[index] = sortedGroupedClasses[0];
                sortedGroupedClasses.RemoveAt(0);


            }

        }

        public void SetUpListOfStartTimes()
        {
            
        }

        public static int FindBestTimeslotFit(TimeSpan startTime)
        {
            // find closest times to start time
                // if multiple free
                    // find the one with the least number of people scheduled that day.
            
        }
        
        public static bool IsFull()
        {
            if (Schedule.Blocks.Length > Schedule.NumberOfTimeSlotsAvailable) // scratch this
            {
                throw new Exception("Too many blocks schedules");
            }
            return (Schedule.Blocks.Length == Schedule.NumberOfTimeSlotsAvailable);
        }
      

        /**************************************************************************\
        Method: SetUpBlocks
        Description: Makes a block for each timeslot available
        \**************************************************************************/
        public void SetUpBlocks()
        {
            this.blocks = new List<Block>(Schedule.NumberOfTimeSlotsAvailable); // intialize blocks to proper size
            for (int i = 0; i < Schedule.NumberOfTimeSlotsAvailable; i++)
            {
                this.blocks.Add(new Block());
            }
        }



        /**************************************************************************\
        Method: SetNumberOfTimeSlotsAvailable
        Description: Calculates number of timeslots available and sets it.
        \**************************************************************************/
        private static void SetNumberOfTimeSlotsAvailable()
        {
            Schedule.numberOfTimeSlotsAvailable = Schedule.NumberOfTimeSlotsAvailablePerDay * Schedule.NumberOfDays;
        }



        // ?
        // needs finishing
        /**************************************************************************\
        Method: Display
        Description: Displays all informations stored in Schedule instance
                     with formatting.
        \**************************************************************************/
        public static void Display()
        {
            Console.WriteLine("Number of Days: {0}", Schedule.NumberOfDays);
            Console.WriteLine("Start Time for Exams: {0}", Schedule.ExamsStartTime);
            Console.WriteLine("Length of Exams: {0}", Schedule.ExamsLength);
            Console.WriteLine("Time Between Exams: {0}", Schedule.TimeBetweenExams);
            Console.WriteLine("Length of Lunch Time: {0}", Schedule.LunchLength);
        }


        /**************************************************************************\
        Method: DisplayBlocks
        Description: Displays all blocks in schedule
        \**************************************************************************/
        public void DisplayBlocks()
        {
            foreach (Block block in this.Blocks)
            {
                Console.WriteLine("Block");
                Console.WriteLine("**************");
                block.Display();
                Console.WriteLine();
            }
        }

        // need to account for lunch ?
        public TimeSpan GetStartTimeOfBlock(int indexOfBlock)
        {
            TimeSpan startTime = Schedule.examsStartTime;
            indexOfBlock %= Schedule.NumberOfTimeSlotsAvailablePerDay;

            for(int i = 0; i < indexOfBlock; i++)
            {
                startTime += Schedule.ExamsLength + Schedule.TimeBetweenExams;
            }
            

        }


        // ? maybe make bool to see if it is read
        // ? this might need to be moved
        // ? catch exception that file couldn't be opened?
        /**************************************************************************\
        Method: readInputConstraintsFile
        Description: Reads in constraints file and intializes static schedule
                     data members
        \**************************************************************************/
        public static void readInputConstraintsFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile); // ?

            Schedule.numberOfDays = Int32.Parse(reader.ReadLine());
            Schedule.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.examsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

            Schedule.SetNumberOfTimeSlotsAvailable();
        }

        public static void SetUpScheduleConstraints(int numberOfDay, TimeSpan examsStartTime, TimeSpan examsLength, TimeSpan timeBetweenExams, TimeSpan lunchLength)
        {
            Schedule.numberOfDays = numberOfDay;
            Schedule.examsStartTime = examsStartTime;
            Schedule.examsLength = examsLength;
            Schedule.timeBetweenExams = timeBetweenExams;
            Schedule.lunchLength = lunchLength;
        }


        // ? maybe make bool to see if it is read
        // ? this might need to be moved
        // ? break up for coehesion and for easier use of manual input
        // Programmer: Ben
        // takes in an open data file and returns a list of all the classes
        /**************************************************************************\
        Method: readInputDataFile
        Description: Reads in data file and constructs list of all classes
                     in the file. Does not add classes in that fall into the
                     criteria of ignorable classes
        \**************************************************************************/
        public static void readInputDataFile(string inFileName)
        {

            // Make boundaries of ignored classes more usable
            TimeSpan ignoreClassLength = TimeSpan.ParseExact(Schedule.CLASS_LENGTH_TO_START_IGNORING, @"hhmm", CultureInfo.InvariantCulture); // can't declare TimeSpan as const so do this here
            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(HOUR_TO_BEGIN_IGNORE_CLASS, @"hhmm", CultureInfo.InvariantCulture);

            // Initialize all classes
            Schedule.allClasses = new List<Class>();

            FileStream inFile = File.OpenRead(@inFileName);
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

                // Checks if class should not be ignored before continuing execution
                if ((startTime < ignoreClassStartTime) // if start time is in range to be considered
                    && (endTime - startTime < ignoreClassLength) // if class length is less than ignorable length
                    && days.Count > 1 // if class occurs on more than one day
                    && ((TimeSpan.Compare(endTime - startTime, TimeSpan.FromMinutes(50))) == 0 // if class length is 50 minutes
                        || (TimeSpan.Compare(endTime - startTime, TimeSpan.FromMinutes(75)) == 0))) // if class length is 1:15
                {
                    Schedule.allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list
                }
            }
        }


       


    } // end class Schedule
}
