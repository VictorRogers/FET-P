using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // TODO: some of these aren't needed

using System.Globalization;  // allows times to be different pased on local TODO: may can be removed
using System.Diagnostics;
using System.IO;


namespace FETP
{
    /// <summary>
    /// Placeholder
    /// </summary>
    public class Schedule
    {
        #region Utilities
        //Rationale: Utilities are supporting functions that are used often such as
        //           Random and others

        //Utility Data Members Here

        //Utility Methods Here

        #endregion


        #region Data Constants
        /// <summary>
        /// Placeholder
        /// </summary>
        private const string CLASS_LENGTH_TO_START_IGNORING = "0126"; // TODO: clean these up

        /// <summary>
        /// Placeholder
        /// </summary>
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";

        /// <summary>
        /// Placeholder
        /// </summary>
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        #endregion


        #region Data Members
        //Static Members
        private static int numberOfDays;
        private static TimeSpan examsStartTime;
        private static TimeSpan examsLength;
        private static TimeSpan timeBetweenExams;
        private static TimeSpan lunchLength;
        private static List<Class> allClasses;
        private static int numberOfTimeSlotsAvailable;
        private static Block[] blocks;

        //Non-static Members

        #endregion


        #region Properties
        /// <summary>
        /// Placeholder
        /// </summary>
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

        /// <summary>
        /// Placeholder
        /// </summary>
        public static int NumberOfDays
        {
            get { return numberOfDays; }
            //set { Schedule.numberOfDays = value; }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static TimeSpan ExamsStartTime
        {
            get { return examsStartTime; }
            //set { Schedule.examsStartTime = value; }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static TimeSpan ExamsLength
        {
            get { return examsLength; }
            //set { Schedule.examsLength = value; }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static TimeSpan TimeBetweenExams
        {
            get { return timeBetweenExams; }
            //set { Schedule.timeBetweenExams = value; }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static TimeSpan LunchLength
        {
            get { return lunchLength; }
            //set { this.lunchLength = value; }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static List<Class> AllClasses
        {
            get
            {
                return Schedule.allClasses;
            }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static int NumberOfTimeSlotsAvailable
        {
            get
            {
                return Schedule.numberOfTimeSlotsAvailable;
            }
            
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static int NumberOfTimeSlotsAvailablePerDay
        {
            get
            {
                TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go // TODO: maybe rewrite

                TimeSpan lengthOfExamDay = latestTime - Schedule.ExamsStartTime; // Figure out how much time available for exams

                // if the lunch time is longer than the break time, account for it and the extra break time it will give you
                if (Schedule.LunchLength > Schedule.TimeBetweenExams)
                {
                    lengthOfExamDay -= (Schedule.LunchLength - Schedule.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
                }

                TimeSpan examFootprint = Schedule.ExamsLength + Schedule.TimeBetweenExams;

                int numberOfExams = 0;
                // TODO: bug if exam break is too big
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

        #endregion


        #region Methods
        //TODO: Possibly make the classes static. It would make the program faster.
        /// <summary>
        /// Placeholder - Currently does nothing
        /// </summary>
        public Schedule() 
        {
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <returns></returns>
        public static bool IsFull()
        {
            if (Schedule.Blocks.Length > Schedule.NumberOfTimeSlotsAvailable)
            {
                throw new Exception("Too many blocks schedules");
            }
            return (Schedule.Blocks.Length == Schedule.NumberOfTimeSlotsAvailable);
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="indexOfBlock"></param>
        /// <returns></returns>
        public TimeSpan GetStartTimeOfBlock(int indexOfBlock)
        {
            TimeSpan startTime = Schedule.examsStartTime;
            indexOfBlock %= Schedule.NumberOfTimeSlotsAvailablePerDay;

            for (int i = 0; i < indexOfBlock; i++)
            {
                startTime += Schedule.ExamsLength + Schedule.TimeBetweenExams;
            }

            return startTime;
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="groupedClasses"></param>
        /// <returns></returns>
        public static List<Block> ScheduleBlocks(List<Block> groupedClasses)
        {
            Schedule.blocks = new Block[Schedule.numberOfTimeSlotsAvailable];
            List<Block> sortedGroupedClasses = groupedClasses.OrderByDescending(c => c.Enrollment).ToList();
            while (sortedGroupedClasses.Count > 0 && !Schedule.IsFull())
            {
                int index = FindBestTimeslotFit(sortedGroupedClasses[0].WeightedAverageStartTime);
                blocks[index] = sortedGroupedClasses[0];
                sortedGroupedClasses.RemoveAt(0);
            }

            return sortedGroupedClasses;
        }


        /// <summary>
        /// Makes a block for each timeslot available
        /// </summary>
        public void SetUpBlocks()
        {
            blocks = new Block[Schedule.NumberOfTimeSlotsAvailable]; // intialize blocks to proper size
            for (int i = 0; i < Schedule.NumberOfTimeSlotsAvailable; i++)
            {
                blocks[i] = new Block();
            }
        }


        /// <summary>
        /// Calculates number of timeslots available and sets it.
        /// </summary>
        private static void SetNumberOfTimeSlotsAvailable()
        {
            Schedule.numberOfTimeSlotsAvailable = Schedule.NumberOfTimeSlotsAvailablePerDay * Schedule.NumberOfDays;
        }


        //TODO: Needs work
        /// <summary>
        /// Displays all information stored in a Schedule instance with formatting.
        /// </summary>
        public static void Display()
        {
            Console.WriteLine("Number of Days: {0}", Schedule.NumberOfDays);
            Console.WriteLine("Start Time for Exams: {0}", Schedule.ExamsStartTime);
            Console.WriteLine("Length of Exams: {0}", Schedule.ExamsLength);
            Console.WriteLine("Time Between Exams: {0}", Schedule.TimeBetweenExams);
            Console.WriteLine("Length of Lunch Time: {0}", Schedule.LunchLength);
        }


        /// <summary>
        /// Displays all blocks in schedule
        /// </summary>
        public void DisplayBlocks()
        {
            foreach (Block block in blocks)
            {
                Console.WriteLine("Block");
                Console.WriteLine("**************");
                block.Display();
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static int FindBestTimeslotFit(TimeSpan startTime)
        {
            //Find closest times to start time
            //if multiple free
            //find the one with the least amount of people scheduled that day
            return 0;
        }


        //TODO: Make bool to see if it's read
        //TODO: This might need to be moved
        //TODO: Catch exception that file couldn't be opened?
        /// <summary>
        /// Reads in the constraints file and initializes a static schedule
        /// </summary>
        /// <param name="inFileName"></param>
        public static void readInputConstraintsFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile);

            Schedule.numberOfDays = Int32.Parse(reader.ReadLine());
            Schedule.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.examsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

            Schedule.SetNumberOfTimeSlotsAvailable();
        }


        /// <summary>
        /// Reads in a data file and constructs list of all classes in the file. Does
        /// not add classes in that fall into the criteria of ignorable classes.
        /// </summary>
        /// <param name="inFileName"></param>
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
                // TODO: possibly change var to string
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

                if ((startTime < ignoreClassStartTime) && (endTime - startTime < ignoreClassLength)
                    && (days.Count > 1) && (TimeSpan.Compare(endTime - startTime, TimeSpan.FromMinutes(50)) == 0))
                {
                    Schedule.allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list
                }
            }
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="numberOfDay"></param>
        /// <param name="examsStartTime"></param>
        /// <param name="examsLength"></param>
        /// <param name="timeBetweenExams"></param>
        /// <param name="lunchLength"></param>
        private void SetupScheduleConstraints(int numberOfDay, TimeSpan examsStartTime,
                                              TimeSpan examsLength, TimeSpan timeBetweenExams,
                                              TimeSpan lunchLength)
        {
            Schedule.numberOfDays = numberOfDay;
            Schedule.examsStartTime = examsStartTime;
            Schedule.examsLength = examsLength;
            Schedule.timeBetweenExams = timeBetweenExams;
            Schedule.lunchLength = lunchLength;

        }

        #endregion


        #region Overloaded Operators
        //Overloaded Operators Here

        #endregion
    }
}
