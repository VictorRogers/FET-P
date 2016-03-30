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
    //TODO: rearrange method positions 
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
        /// Upper Limit of length of classes
        /// </summary>
        private const string CLASS_LENGTH_TO_START_IGNORING = "0125"; // TODO: clean these up

        /// <summary>
        /// Start point to begin ignoring classes
        /// </summary>
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";

        //TODO: figure out access levels
        /// <summary>
        /// Latest exams can go
        /// </summary>
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        //TODO: figure these out
        public const string LOWER_TIME_RANGE_FOR_LUNCH = "1100";
        public const string UPPER_TIME_RANGE_FOR_LUNCH = "0100";

        #endregion


        #region Data Members
        //Static Members
        /// <summary>
        /// Number of days available for exams
        /// </summary>
        private int numberOfDays;

        /// <summary>
        /// Time for exams to start
        /// </summary>
        private TimeSpan examsStartTime;

        /// <summary>
        /// Length of each exam
        /// </summary>
        private TimeSpan examsLength;

        /// <summary>
        /// Time between each exam (break time)
        /// </summary>
        private TimeSpan timeBetweenExams;

        /// <summary>
        /// Length of the lunch break
        /// </summary>
        private TimeSpan lunchLength;

        /// <summary>
        /// List of all classes to be scheduled in the order read in
        /// </summary>
        private List<Class> allClasses;

        /// TODO: figure out how we want to do these datatypes
        private int numberOfTimeSlotsAvailable;

        /// <summary>
        /// Array of all blocks (grouped classes) scheduled
        /// </summary>
        private Block[] blocks;

        private List<Block> leftoverBlocks;

        private TimeSpan[] startTimesOfExams;

        //Non-static Members

        #endregion


        #region Properties
        /// <summary>
        /// Getter propertie for array of all blocks (grouped classes) scheduled
        /// </summary>
        public Block[] Blocks
        {
            get
            {
                return this.blocks;
            }
        }

        /// <summary>
        /// Getter property for the number of exams days available for scheduling
        /// </summary>
        public int NumberOfDays
        {
            get
            {
                return numberOfDays;
            }
        }

        /// <summary>
        /// Getter property for time for exams to start
        /// </summary>
        public TimeSpan ExamsStartTime
        {
            get
            {
                return examsStartTime;
            }
        }

        /// <summary>
        /// Getter property for the length of each exam
        /// </summary>
        public TimeSpan ExamsLength
        {
            get
            {
                return examsLength;
            }
        }

        /// <summary>
        /// Getter property for the time between each exam (break time)
        /// </summary>
        public TimeSpan TimeBetweenExams
        {
            get
            {
                return timeBetweenExams;
            }
        }

        /// <summary>
        /// Getter property for the length of the lunch period
        /// </summary>
        public TimeSpan LunchLength
        {
            get
            {
                return lunchLength;
            }
        }

        /// <summary>
        /// Getter property for the list of all classes to be scheduled
        /// </summary>
        public List<Class> AllClasses
        {
            get
            {
                return this.allClasses;
            }
        }

        /// <summary>
        /// Getter property for the number of Timeslots available in total
        /// </summary>
        public int NumberOfTimeSlotsAvailable
        {
            get
            {
                return this.numberOfTimeSlotsAvailable;
            }
        }
        // TODO: convert to data member for faster speed
        /// <summary>
        /// Placeholder
        /// </summary>
        public int NumberOfTimeSlotsAvailablePerDay
        {
            get
            {
                TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go // TODO: maybe rewrite

                TimeSpan lengthOfExamDay = latestTime - this.ExamsStartTime; // Figure out how much time available for exams

                // if the lunch time is longer than the break time, account for it and the extra break time it will give you
                if (this.LunchLength > this.TimeBetweenExams)
                {
                    lengthOfExamDay -= (this.LunchLength - this.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
                }

                TimeSpan examFootprint = this.ExamsLength + this.TimeBetweenExams;

                int numberOfExams = 0;
                // TODO: bug if exam break is too big
                while ((lengthOfExamDay - this.ExamsLength) >= TimeSpan.Zero)
                {
                    lengthOfExamDay -= this.ExamsLength;
                    numberOfExams++;

                    // checks if a break is needed due to their being room for another exam after a break
                    if ((lengthOfExamDay - (this.TimeBetweenExams + this.ExamsLength) >= TimeSpan.Zero))
                    {
                        lengthOfExamDay -= this.TimeBetweenExams;
                    }
                }
                return numberOfExams;
            }
        }

        #endregion


        #region Methods
        //TODO: write subfunction for constructor to avoid rewriting the code

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFileAddress"></param>
        /// <param name="constraintsFileAddress"></param>
        public Schedule(string dataFileAddress, string constraintsFileAddress)
        {
            // Intial Setup
            this.readInputDataFile(dataFileAddress);
            this.readInputConstraintsFile(constraintsFileAddress);
            SetNumberOfTimeSlotsAvailable();

            this.SetupExamStartTimeTable();

            this.ScheduleBlocks(FETP_Controller.GroupClasses(this.AllClasses));

        }

        //TODO: clean up constructors. i really don't know how else to word it
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFileAddress"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="examsStartTime"></param>
        /// <param name="examsLength"></param>
        /// <param name="timeBetweenExams"></param>
        /// <param name="lunchLength"></param>
        public Schedule(string dataFileAddress, int numberOfDays, TimeSpan examsStartTime,
                        TimeSpan examsLength, TimeSpan timeBetweenExams, TimeSpan lunchLength)
        {
            //Intial setup
            this.readInputConstraintsFile(dataFileAddress);
            this.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);
            SetNumberOfTimeSlotsAvailable();

            this.SetupExamStartTimeTable();

            this.ScheduleBlocks(FETP_Controller.GroupClasses(this.AllClasses));

        }


        //TODO: rewrite cleaner
        /// <summary>
        /// Placeholder
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            foreach (Block block in blocks)
                if (block == null) return false;
            return true;
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="indexOfBlock"></param>
        /// <returns></returns>
        public TimeSpan GetStartTimeOfBlock(int indexOfBlock)
        {
            TimeSpan startTime = this.examsStartTime;
            indexOfBlock %= this.NumberOfTimeSlotsAvailablePerDay;

            for (int i = 0; i < indexOfBlock; i++)
            {
                startTime += this.ExamsLength + this.TimeBetweenExams;
            }

            return startTime;
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="groupedClasses"></param>
        /// <returns></returns>
        public void ScheduleBlocks(List<Block> groupedClasses)
        {
            this.blocks = new Block[this.numberOfTimeSlotsAvailable];
            this.leftoverBlocks = groupedClasses.OrderByDescending(c => c.Enrollment).ToList();
            while (this.leftoverBlocks.Count > 0 && !this.IsFull())
            {
                leftoverBlocks[0].Display();
                int index = this.FindBestTimeslotFit(this.leftoverBlocks[0].WeightedAverageStartTime);
                this.blocks[index] = leftoverBlocks[0];
                this.leftoverBlocks.RemoveAt(0);
                blocks[index].Display();
            }

        }

        //TODO: NUKE?
        /// <summary>
        /// Makes a block for each timeslot available
        /// </summary>
        public void SetUpBlocks()
        {
            blocks = new Block[this.NumberOfTimeSlotsAvailable]; // intialize blocks to proper size
            for (int i = 0; i < this.NumberOfTimeSlotsAvailable; i++)
            {
                blocks[i] = new Block();
            }
        }

        // move into constructors
        /// <summary>
        /// Calculates number of timeslots available and sets it.
        /// </summary>
        private void SetNumberOfTimeSlotsAvailable()
        {
            this.numberOfTimeSlotsAvailable = this.NumberOfTimeSlotsAvailablePerDay * this.NumberOfDays;
        }


        //TODO: Needs work
        /// <summary>
        /// Displays all information stored in a Schedule instance with formatting.
        /// </summary>
        public void Display()
        {
            Console.WriteLine("\n*******************************");
            Console.WriteLine("DISPLAYING SCHEDULE INFORMATION");
            Console.WriteLine("*******************************");
            Console.WriteLine("Number of Days: {0}", this.NumberOfDays);
            Console.WriteLine("Start Time for Exams: {0}", this.ExamsStartTime);
            Console.WriteLine("Length of Exams: {0}", this.ExamsLength);
            Console.WriteLine("Time Between Exams: {0}", this.TimeBetweenExams);
            Console.WriteLine("Length of Lunch Time: {0}", this.LunchLength);
            Console.WriteLine("Number of Timeslots Per Day: {0}", this.NumberOfTimeSlotsAvailablePerDay);
            Console.WriteLine("Number of Timeslots: {0}", this.NumberOfTimeSlotsAvailable);
            Console.WriteLine("*******************************\n");
        }


        /// <summary>
        /// Displays all blocks in schedule
        /// </summary>
        public void DisplayBlocks()
        {
            Console.WriteLine("\n*****************************");
            Console.WriteLine("DISPLAYING BLOCKS INFORMATION");
            Console.WriteLine("*****************************");
            foreach (Block block in blocks)
            {
                if( block != null)
                {
                    Console.WriteLine("Block");
                    Console.WriteLine("**************");
                    block.Display();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("NOT GOOD: IN DisplayBlocks");
                }
                
            }
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public int FindBestTimeslotFit(TimeSpan startTime)
        {

            // create ordered list of times
            // order by closeness to input start time, then by which one is later
            // it's weird but easiest way
            //TODO: clean up this
            List<TimeSpan> orderedPossibleTime = this.startTimesOfExams.ToList().OrderBy(c => (c - startTime)).ThenByDescending(c => c.Ticks).ToList();
            List<int> orderedIndexesOfPossibleTime = new List<int>();
            // sets up ordered list of all indexes 
            foreach(TimeSpan time in orderedPossibleTime)
            {
                // for each index of time, add all indexes with that time to list
                for(int i = GetIndexOfStartTime(time); i < this.blocks.Length; i += this.NumberOfTimeSlotsAvailablePerDay) 
                {
                    orderedIndexesOfPossibleTime.Add(i);
                }
            }

            //TODO: clean this up, should still work for the mean time
            // ? could have exception for failing to return. that should never happen
            // finds the first possible index in ordered list of best indexes that is empty
            foreach (int index in orderedIndexesOfPossibleTime)
            {
                if (this.Blocks.ElementAt(index) == null)
                    return index;
            }

            //TODO: this is bad
            Console.WriteLine("NOT GOOD: IN FindBestTimeslotFit");
            return 0;
        }


        //TODO: figure out cleaner way to do this. it needs to be converted to list and sorted but that loses indexes
        private int GetIndexOfStartTime(TimeSpan startTime)
        {
            int i = 0;
            while (i < this.startTimesOfExams.Length)
            {
                if (startTimesOfExams[i] == startTime) return i;
                i++;
            }

            //TODO: this is bad
            Console.WriteLine("NOT GOOD: IN GetIndexOfStartTime");
            return 0;
           
        }


        //TODO: Make bool to see if it's read
        //TODO: This might need to be moved
        //TODO: Catch exception that file couldn't be opened?
        /// <summary>
        /// Reads in the constraints file and initializes a static schedule
        /// </summary>
        /// <param name="inFileName"></param>
        public void readInputConstraintsFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile);

            this.numberOfDays = Int32.Parse(reader.ReadLine());
            this.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            this.examsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            this.timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            this.lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

            this.SetNumberOfTimeSlotsAvailable();
        }


        /// <summary>
        /// Reads in a data file and constructs list of all classes in the file. Does
        /// not add classes in that fall into the criteria of ignorable classes.
        /// </summary>
        /// <param name="inFileName"></param>
        public void readInputDataFile(string inFileName)
        {

            // Make boundaries of ignored classes more usable
            TimeSpan ignoreClassLength = TimeSpan.ParseExact(Schedule.CLASS_LENGTH_TO_START_IGNORING, @"hhmm", CultureInfo.InvariantCulture); // can't declare TimeSpan as const so do this here
            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(Schedule.HOUR_TO_BEGIN_IGNORE_CLASS, @"hhmm", CultureInfo.InvariantCulture);

            // Initialize all classes
            this.allClasses = new List<Class>();

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

                if ((startTime <= ignoreClassStartTime) && (endTime - startTime < ignoreClassLength)
                    && (days.Count > 1) && (TimeSpan.Compare(endTime - startTime, TimeSpan.FromMinutes(50)) == 0))
                {
                    this.allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list
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
        public void SetupScheduleConstraints(int numberOfDay, TimeSpan examsStartTime,
                                              TimeSpan examsLength, TimeSpan timeBetweenExams,
                                              TimeSpan lunchLength)
        {
            this.numberOfDays = numberOfDay;
            this.examsStartTime = examsStartTime;
            this.examsLength = examsLength;
            this.timeBetweenExams = timeBetweenExams;
            this.lunchLength = lunchLength;

        }

        /// <summary>
        /// 
        /// </summary>
        public void SetupExamStartTimeTable()
        {
            // setup lower limir for lunch from constant for easier use
            TimeSpan lowerLimitForLunch = TimeSpan.ParseExact(Schedule.LOWER_TIME_RANGE_FOR_LUNCH, @"hhmm", CultureInfo.InvariantCulture);

            this.startTimesOfExams = new TimeSpan[this.NumberOfTimeSlotsAvailablePerDay]; // intialize start times table

            int index = 0;
            bool isLunchPast = false; // accounts for exams after lunch to allow lunch time to be added to their start times
            while (index < startTimesOfExams.Length)
            {
                startTimesOfExams[index] = this.ExamsStartTime + TimeSpan.FromTicks((this.ExamsLength.Ticks + this.TimeBetweenExams.Ticks) * index);

                //TODO: could be error if lunch time isn't in this time range, or just an error anywhere in it, or if exam lengths too long
                // if need to account for lunch
                if (this.LunchLength > this.timeBetweenExams)
                {
                    if (isLunchPast) // lunch period has passed
                    {
                        this.startTimesOfExams[index] += (this.LunchLength - this.TimeBetweenExams); // account for lunch time having replaced a break
                    }
                    else if (this.startTimesOfExams[index] + this.ExamsLength >= lowerLimitForLunch) // if exam ends in the range of lunch, lunch will immidiatly follow
                    {
                        isLunchPast = true;
                    }
                }
                index++;
            }
        }

 
        //public void ScheduleLunch()
        //{
        //    if (this.LunchLength > this.TimeBetweenExams)
        //    {

        //        TimeSpan lowerLimitForLunch = TimeSpan.ParseExact(Schedule.LOWER_TIME_RANGE_FOR_LUNCH, @"hhmm", CultureInfo.InvariantCulture);
        //        int index = 0;
        //        bool isLunchFound = false;
        //        while (index < this.startTimesOfExams.Length) // while we haven't gone through the whole schedule
        //        {
                    

        //            index++;

                       
        //        }
        //        this.startTimesOfExams
        //    }
        //}


        #endregion


        #region Overloaded Operators
        //Overloaded Operators Here

        #endregion
    }
}
