﻿//NULL BLOCKS AT END OF SCHEDULE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Globalization;  // allows times to be different pased on local TODO: may can be removed
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // TODO: some of these aren't needed 


namespace FETP
{
    #region NULL BLOCKS AT END OF SCHEDULE

    //TODO: rearrange method positions 
    /// <summary>
    /// Placeholder
    /// </summary>
    [Serializable]
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
        private const string CLASS_LENGTH_TO_START_IGNORING = "0125"; // TODO: clean these up // it may be possibly to make them easy to modify and in the right format for easy use

        /// <summary>
        /// Start point to begin ignoring classes
        /// </summary>
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";

        //TODO: figure out access levels // if functions/data members can be private, make them private
        /// <summary>
        /// Latest exams can go
        /// </summary>
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        //TODO: figure these out // not sure which is still needed for lunch time scheduling. Lunch time scheduling still needs work // TimeSpan cannot be const
        public const string LOWER_TIME_RANGE_FOR_LUNCH = "1100";
        public const string UPPER_TIME_RANGE_FOR_LUNCH = "0100";

        public const int MAX_NUMBER_OF_DAYS_FOR_EXAMS = 7;
        public const int MIN_NUMBER_OF_DAYS_FOR_EXAMS = 1;

        public const string MIN_START_TIME = "0700";
        public const string MAX_START_TIME = "1600"; // TODO: figure these out

        public const int MIN_EXAM_LENGTH_IN_MINUTES = 90;
        public const int MAX_EXAM_LENGTH_IN_MINUTES = 120; // TODO: figure these out

        public const int MIN_BREAK_TIME_IN_MINUTES = 10;
        public const int MAX_BREAK_TIME_IN_MINUTES = 30;

        public const int MIN_LUNCH_LENGTH_IN_MINUTES = 0;
        public const int MAX_LUNCH_LENGTH_IN_MINUTES = 60;




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

        //TODO: figure out how we want to do these datatypes // not sure if this formmating is the best. but they need commentation
        private int numberOfTimeSlotsAvailable;
        private int numberOfTimeSlotsAvailablePerDay;
        private int numberOfTimeSlotsToBeUsed;

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

        /// <summary>
        /// Getter property for number of Timeslots available per day
        /// </summary>
        public int NumberOfTimeSlotsAvailablePerDay
        {
            get
            {
                return this.numberOfTimeSlotsAvailablePerDay;
            }
        }

        //TODO: write comments
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfTimeSlotsToBeUsed
        {
            get
            {
                return this.numberOfTimeSlotsToBeUsed;
            }
        }

        /// <summary>
        /// Getter property for array of all exams's strt times
        /// </summary>
        public TimeSpan[] StartTimesOfExams
        {
            get
            {
                return this.startTimesOfExams;
            }
        }

        /// <summary>
        /// Getter property for list of leftover blocks
        /// </summary>
        public List<Block> LeftoverBlocks
        {
            get
            {
                return this.leftoverBlocks;
            }
        }

        #endregion


        #region Methods
        /// <summary>
        /// </summary>
        public Schedule(string path)
        {
        }


        //TODO: write subfunction for constructor to avoid rewriting the code between constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFileAddress"></param>
        /// <param name="constraintsFileAddress"></param>
        public Schedule(string dataFileAddress, string constraintsFileAddress)
        {
            // Intial Setup
            this.SetupScheduleConstraintsFromFile(constraintsFileAddress);
            this.SetupClassDataFromFile(dataFileAddress);

            this.SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does // maybe

            this.SetupExamStartTimeTable();

            //TODO: Cleanup this section
            this.leftoverBlocks = FETP_Controller.GroupClasses(this.AllClasses);

            if (LeftoverBlocks.Count < NumberOfTimeSlotsAvailable)
            {
                this.numberOfTimeSlotsToBeUsed = LeftoverBlocks.Count;
            }
            else
            {
                this.numberOfTimeSlotsToBeUsed = NumberOfTimeSlotsAvailable;
            }

            this.ScheduleBlocks(this.LeftoverBlocks);

        }

        //TODO: clean up constructors. i really don't know how else to word it
        //TODO: change inputs of constructor all to string to remove conversion work from front end // COMFIRM WITH FRONT END BEFORE DOING THIS
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
            this.SetupClassDataFromFile(dataFileAddress);
            this.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);

            this.SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does

            this.SetupExamStartTimeTable();

            this.leftoverBlocks = FETP_Controller.GroupClasses(this.AllClasses);

            if (LeftoverBlocks.Count < NumberOfTimeSlotsAvailable)
            {
                this.numberOfTimeSlotsToBeUsed = LeftoverBlocks.Count;
            }
            else
            {
                this.numberOfTimeSlotsToBeUsed = NumberOfTimeSlotsAvailable;
            }

            this.ScheduleBlocks(this.LeftoverBlocks);

        }

        //TODO: clean up constructors. i really don't know how else to word it
        //TODO: change inputs of constructor all to string to remove conversion work from front end // COMFIRM WITH FRONT END BEFORE DOING THIS //Confirmed just taking strings
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFileAddress"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="examsStartTime"></param>
        /// <param name="examsLength"></param>
        /// <param name="timeBetweenExams"></param>
        /// <param name="lunchLength"></param>
        public Schedule(string dataFileAddress, string numberOfDays, string examsStartTime,
                        string examsLength, string timeBetweenExams, string lunchLength)
        {
            //Intial setup
            this.SetupClassDataFromFile(dataFileAddress);
            this.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);

            this.SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does

            this.SetupExamStartTimeTable();

            this.leftoverBlocks = FETP_Controller.GroupClasses(this.AllClasses);

            if (LeftoverBlocks.Count < NumberOfTimeSlotsAvailable)
            {
                this.numberOfTimeSlotsToBeUsed = LeftoverBlocks.Count;
            }
            else
            {
                this.numberOfTimeSlotsToBeUsed = NumberOfTimeSlotsAvailable;
            }

            this.ScheduleBlocks(this.LeftoverBlocks);
        }

        //TODO: substitue this function in for code snippets
        private void SetupNumberOfTimeslotsNeeded()
        {
            if (LeftoverBlocks.Count < NumberOfTimeSlotsAvailable)
            {
                this.numberOfTimeSlotsToBeUsed = LeftoverBlocks.Count;
            }
            else
            {
                this.numberOfTimeSlotsToBeUsed = NumberOfTimeSlotsAvailable;
            }
        }

        /// <summary>
        /// Sets up data member examsStartTimes to be used for quickly finding the start time of an index
        /// </summary>
        private void SetupExamStartTimeTable()
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

        //TODO: further investigate what makes the best time
        /// <summary>
        /// Finds the index of the best fit for the block.
        /// This is done by first creating a list of all
        /// indexes in order of best fit then finding 
        /// the first empty spot.
        /// </summary>
        /// <param name="startTime">average start time of block to schedule</param>
        /// <returns>Index of best possible fit</returns>
        private int FindBestTimeslotFit(TimeSpan startTime)
        {

            // create ordered list of times
            // order by closeness to input start time, then by which one is later
            // it's weird but easiest way
            //TODO: clean up this
            //TODO: look into reordering to find most valuable times more weighted by the enrollment numbers of that day.
            List<TimeSpan> orderedPossibleTime = this.startTimesOfExams.ToList().OrderBy(c => (c - startTime)).ThenByDescending(c => c.Ticks).ToList();
            List<int> orderedIndexesOfPossibleTime = new List<int>();
            // sets up ordered list of all indexes 
            foreach (TimeSpan time in orderedPossibleTime)
            {
                // for each index of time, add all indexes with that time to list
                for (int i = GetIndexOfStartTime(time); i < this.NumberOfTimeSlotsToBeUsed; i += this.NumberOfTimeSlotsAvailablePerDay)
                {
                    orderedIndexesOfPossibleTime.Add(i);
                }
            }

            //TODO: clean this up, should still work for the mean time
            // ? could have exception for failing to return. that should never happen
            // finds the first possible index in ordered list of best indexes that is empty
            bool wasFound = false;
            int indexOfIndex = 0;
            while (indexOfIndex < orderedIndexesOfPossibleTime.Count && !wasFound)
            {
                if (this.Blocks.ElementAt(orderedIndexesOfPossibleTime[indexOfIndex]) == null) // if the spot at the index is empty
                {
                    wasFound = true;
                }
                else
                {
                    indexOfIndex++;
                }
            }

            if (!wasFound)
            {
                throw new Exception("Best Fit not found in FindBestTimslotFit.");
            }
            else
            {
                return orderedIndexesOfPossibleTime[indexOfIndex];
            }

            //foreach (int index in orderedIndexesOfPossibleTime)
            //{
            //    if (this.Blocks.ElementAt(index) == null)
            //        return index;
            //}

            //Console.WriteLine("NOT GOOD: IN FindBestTimeslotFit");
            //return 0;
        }


        //TODO: figure out cleaner way to do this. it needs to be converted to list and sorted but that loses indexes
        /// <summary>
        /// Finds the index of input start time in table of start times
        /// </summary>
        /// <param name="startTime">Start time whose index is desired.</param>
        /// <returns></returns>
        private int GetIndexOfStartTime(TimeSpan startTime)
        {
            bool wasFound = false;
            int i = 0;
            while (i < this.startTimesOfExams.Length && !wasFound)
            {
                if (startTimesOfExams[i] == startTime)
                {
                    wasFound = true;
                }
                else
                {
                    i++;
                }
            }
            if (!wasFound)
            {
                throw new Exception("Invalid input start time. No valid start time was found.");
            }
            else
            {
                return i;
            }

            //////TODO: this is bad
            ////Console.WriteLine("NOT GOOD: IN GetIndexOfStartTime");
            ////return 0;

        }

        /// <summary>
        /// Determines if blocks has any empty spaces
        /// </summary>
        /// <returns>Whether or not empty spaces exist</returns>
        public bool IsFull()
        {
            bool isFull = true;
            int i = 0;
            while (i < blocks.Length && isFull)
            {
                if (blocks[i] == null)
                {
                    isFull = false;
                }
                i++;
            }
            return isFull;
        }


        /// <summary>
        /// Gets the start time of exam block
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
        private void ScheduleBlocks(List<Block> groupedClasses)
        {
            this.blocks = new Block[this.numberOfTimeSlotsAvailable];
            this.leftoverBlocks = groupedClasses.OrderByDescending(c => c.Enrollment).ToList();
            while (this.leftoverBlocks.Count > 0 && !this.IsFull())
            {
                int index = this.FindBestTimeslotFit(this.leftoverBlocks[0].WeightedAverageStartTime);
                this.blocks[index] = leftoverBlocks[0];
                this.leftoverBlocks.RemoveAt(0);
            }
        }



        //TODO: move into constructors maybe // may already be done
        /// <summary>
        /// Calculates number of timeslots available and sets it.
        /// </summary>
        private void SetNumberOfTimeSlotsAvailable()
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
            this.numberOfTimeSlotsAvailablePerDay = numberOfExams;

            this.numberOfTimeSlotsAvailable = this.NumberOfTimeSlotsAvailablePerDay * this.NumberOfDays;
        }


        //TODO: Needs work
        // ? gut these display methods before shipping
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
            Console.WriteLine("\n***************************************");
            Console.WriteLine("DISPLAYING SCHEDULED BLOCKS INFORMATION");
            Console.WriteLine("***************************************");
            for (int i = 0; i < this.blocks.Length; i++)
            {
                if (blocks[i] != null)
                {

                    Console.WriteLine("**************");
                    Console.WriteLine("Block");
                    Console.WriteLine("**************");
                    Console.WriteLine("Block start time: {0}", this.startTimesOfExams[i % this.NumberOfTimeSlotsAvailablePerDay]);
                    blocks[i].Display();
                    Console.WriteLine("\nClasses In Block");
                    Console.WriteLine("********");

                    blocks[i].DisplayAllClasses();
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine("==============");
                    Console.WriteLine("EMPTY BLOCK");
                    Console.WriteLine("==============");
                }

            }

            Console.WriteLine("\n*******************************************");
            Console.WriteLine("DISPLAYING NON SCHEDULED BLOCKS INFORMATION");
            Console.WriteLine("*******************************************");
            foreach (Block block in this.leftoverBlocks)
            {
                Console.WriteLine("**************");
                Console.WriteLine("Block");
                Console.WriteLine("**************");
                block.Display();
                Console.WriteLine("\nClasses In Block");
                Console.WriteLine("********");
                block.DisplayAllClasses();
                Console.WriteLine();
            }
        }


        //TODO: Make bool to see if it's read
        //TODO: This might need to be moved
        //TODO: Catch exception that file couldn't be opened?
        //TODO: modify to not parse. Make it read the file then pass off to SetupScheduleConstraints once the function is modified to take only strings and parse. 
        /// <summary>
        /// Reads in the constraints file and initializes a static schedule
        /// </summary>
        /// <param name="inFileName"></param>
        private void SetupScheduleConstraintsFromFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile);

            //TODO: possibly implement TryParse or other form of error handling
            this.numberOfDays = Int32.Parse(reader.ReadLine());
            this.examsStartTime = TimeSpan.FromMinutes(Int32.Parse(reader.ReadLine())); //TODO: further investigate CultureInfo.InvariantCulture to be sure it's needed and doesn't break stuff
            this.examsLength = TimeSpan.FromMinutes(Int32.Parse(reader.ReadLine()));
            this.timeBetweenExams = TimeSpan.FromMinutes(Int32.Parse(reader.ReadLine())); //TODO: Test to make sure the from minutes functions with CultureInfo.InvariantCulture
            this.lunchLength = TimeSpan.FromMinutes(Int32.Parse(reader.ReadLine()));

        }


        /// <summary>
        /// Reads in a data file and constructs list of all classes in the file. Does
        /// not add classes in that fall into the criteria of ignorable classes.
        /// </summary>
        /// <param name="inFileName"></param>
        private void SetupClassDataFromFile(string inFileName)
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
        /// Takes in data for the constraints required to design a schedule
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
            this.numberOfDays = numberOfDay;
            this.examsStartTime = examsStartTime;
            this.examsLength = examsLength;
            this.timeBetweenExams = timeBetweenExams;
            this.lunchLength = lunchLength;
        }

        private void SetupScheduleConstraints(string numberOfDays, string examsStartTime,
                                              string examsLength, string timeBetweenExams,
                                              string lunchLength)
        {
            this.numberOfDays = Int32.Parse(numberOfDays);
            DateTime tempStartTime = Convert.ToDateTime(examsStartTime);
            this.examsStartTime = TimeSpan.FromTicks(tempStartTime.TimeOfDay.Ticks);
            this.examsLength = TimeSpan.FromMinutes(Int32.Parse(examsLength));
            this.timeBetweenExams = TimeSpan.FromMinutes(Int32.Parse(timeBetweenExams)); //TODO: Test to make sure the from minutes functions with CultureInfo.InvariantCulture
            this.lunchLength = TimeSpan.FromMinutes(Int32.Parse(lunchLength));
        }

        private void SetupScheduleConstraints(int numberOfDays, int examsStartTime,
                                              int examsLength, int timeBetweenExams,
                                              int lunchLength)
        {
            this.numberOfDays = numberOfDays;
            this.examsStartTime = TimeSpan.FromMinutes(examsStartTime); //TODO: further investigate CultureInfo.InvariantCulture to be sure it's needed and doesn't break stuff
            this.examsLength = TimeSpan.FromMinutes(examsLength);
            this.timeBetweenExams = TimeSpan.FromMinutes(timeBetweenExams); //TODO: Test to make sure the from minutes functions with CultureInfo.InvariantCulture
            this.lunchLength = TimeSpan.FromMinutes(lunchLength);
        }



        /// <summary>
        /// 
        /// </summary>
        public void SaveSchedule(string path)
        {
            FileStream stream = File.Create(path);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();
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
    #endregion
}


////--------------------------------------------------------------------------------------------------------------------------------------------------


//SCATTERED NULL BLOCKS
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Globalization;  // allows times to be different pased on local TODO: may can be removed
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks; // TODO: some of these aren't needed 
//using System.Xml.Serialization;


//namespace FETP
//{
//    #region SCATTERED NULL BLOCKS
//    //TODO: rearrange method positions 
//    /// <summary>
//    /// Placeholder
//    /// </summary>
//    public class Schedule
//    {
//        #region Utilities
//        //Rationale: Utilities are supporting functions that are used often such as
//        //           Random and others

//        //Utility Data Members Here

//        //Utility Methods Here

//        #endregion


//        #region Data Constants
//        /// <summary>
//        /// Upper Limit of length of classes
//        /// </summary>
//        private const string CLASS_LENGTH_TO_START_IGNORING = "0125"; // TODO: clean these up

//        /// <summary>
//        /// Start point to begin ignoring classes
//        /// </summary>
//        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";

//        //TODO: figure out access levels
//        /// <summary>
//        /// Latest exams can go
//        /// </summary>
//        public const string TIME_EXAMS_MUST_END_BY = "1700";

//        //TODO: figure these out
//        public const string LOWER_TIME_RANGE_FOR_LUNCH = "1100";
//        public const string UPPER_TIME_RANGE_FOR_LUNCH = "0100";

//        #endregion


//        #region Data Members
//        //Static Members
//        /// <summary>
//        /// Number of days available for exams
//        /// </summary>
//        private int numberOfDays;

//        /// <summary>
//        /// Time for exams to start
//        /// </summary>
//        private TimeSpan examsStartTime;

//        /// <summary>
//        /// Length of each exam
//        /// </summary>
//        private TimeSpan examsLength;

//        /// <summary>
//        /// Time between each exam (break time)
//        /// </summary>
//        private TimeSpan timeBetweenExams;

//        /// <summary>
//        /// Length of the lunch break
//        /// </summary>
//        private TimeSpan lunchLength;

//        /// <summary>
//        /// List of all classes to be scheduled in the order read in
//        /// </summary>
//        private List<Class> allClasses;

//        /// TODO: figure out how we want to do these datatypes
//        private int numberOfTimeSlotsAvailable;
//        private int numberOfTimeSlotsAvailablePerDay;

//        /// <summary>
//        /// Array of all blocks (grouped classes) scheduled
//        /// </summary>
//        private Block[] blocks;

//        private List<Block> leftoverBlocks;

//        private TimeSpan[] startTimesOfExams;

//        //Non-static Members

//        #endregion


//        #region Properties
//        /// <summary>
//        /// Getter propertie for array of all blocks (grouped classes) scheduled
//        /// </summary>
//        public Block[] Blocks
//        {
//            get
//            {
//                return this.blocks;
//            }
//        }

//        /// <summary>
//        /// Getter property for the number of exams days available for scheduling
//        /// </summary>
//        public int NumberOfDays
//        {
//            get
//            {
//                return numberOfDays;
//            }
//        }

//        /// <summary>
//        /// Getter property for time for exams to start
//        /// </summary>
//        public TimeSpan ExamsStartTime
//        {
//            get
//            {
//                return examsStartTime;
//            }
//        }

//        /// <summary>
//        /// Getter property for the length of each exam
//        /// </summary>
//        public TimeSpan ExamsLength
//        {
//            get
//            {
//                return examsLength;
//            }
//        }

//        /// <summary>
//        /// Getter property for the time between each exam (break time)
//        /// </summary>
//        public TimeSpan TimeBetweenExams
//        {
//            get
//            {
//                return timeBetweenExams;
//            }
//        }

//        /// <summary>
//        /// Getter property for the length of the lunch period
//        /// </summary>
//        public TimeSpan LunchLength
//        {
//            get
//            {
//                return lunchLength;
//            }
//        }

//        /// <summary>
//        /// Getter property for the list of all classes to be scheduled
//        /// </summary>
//        public List<Class> AllClasses
//        {
//            get
//            {
//                return this.allClasses;
//            }
//        }

//        /// <summary>
//        /// Getter property for the number of Timeslots available in total
//        /// </summary>
//        public int NumberOfTimeSlotsAvailable
//        {
//            get
//            {
//                return this.numberOfTimeSlotsAvailable;
//            }
//        }

//        // TODO: convert to data member for faster speed
//        /// <summary>
//        /// Getter property for number of Timeslots available per day
//        /// </summary>
//        public int NumberOfTimeSlotsAvailablePerDay
//        {
//            get
//            {
//                return this.numberOfTimeSlotsAvailablePerDay;
//            }
//        }

//        /// <summary>
//        /// Getter property for array of all exams's strt times
//        /// </summary>
//        public TimeSpan[] StartTimesOfExams
//        {
//            get
//            {
//                return this.startTimesOfExams;
//            }
//        }

//        /// <summary>
//        /// Getter property for list of leftover blocks
//        /// </summary>
//        public List<Block> LeftoverBlocks
//        {
//            get
//            {
//                return this.leftoverBlocks;
//            }
//        }

//        #endregion


//        #region Methods
//        /// <summary>
//        /// This parameterless constructor is needed for XML serialization
//        /// </summary>
//        private Schedule()
//        {

//        }


//        //TODO: write subfunction for constructor to avoid rewriting the code

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="dataFileAddress"></param>
//        /// <param name="constraintsFileAddress"></param>
//        public Schedule(string dataFileAddress, string constraintsFileAddress)
//        {
//            // Intial Setup
//            this.readInputDataFile(dataFileAddress);
//            this.readInputConstraintsFile(constraintsFileAddress);

//            SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does

//            this.SetupExamStartTimeTable();

//            this.ScheduleBlocks(FETP_Controller.GroupClasses(this.AllClasses));

//        }

//        //TODO: clean up constructors. i really don't know how else to word it
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="dataFileAddress"></param>
//        /// <param name="numberOfDays"></param>
//        /// <param name="examsStartTime"></param>
//        /// <param name="examsLength"></param>
//        /// <param name="timeBetweenExams"></param>
//        /// <param name="lunchLength"></param>
//        public Schedule(string dataFileAddress, int numberOfDays, TimeSpan examsStartTime,
//                        TimeSpan examsLength, TimeSpan timeBetweenExams, TimeSpan lunchLength)
//        {
//            //Intial setup
//            this.readInputDataFile(dataFileAddress);
//            this.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);

//            this.SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does

//            this.SetupExamStartTimeTable();

//            this.ScheduleBlocks(FETP_Controller.GroupClasses(this.AllClasses));

//        }

//        /// <summary>
//        /// Sets up data member examsStartTimes to be used for quickly finding the start time of an index
//        /// </summary>
//        public void SetupExamStartTimeTable()
//        {
//            // setup lower limir for lunch from constant for easier use
//            TimeSpan lowerLimitForLunch = TimeSpan.ParseExact(Schedule.LOWER_TIME_RANGE_FOR_LUNCH, @"hhmm", CultureInfo.InvariantCulture);

//            this.startTimesOfExams = new TimeSpan[this.NumberOfTimeSlotsAvailablePerDay]; // intialize start times table

//            int index = 0;
//            bool isLunchPast = false; // accounts for exams after lunch to allow lunch time to be added to their start times
//            while (index < startTimesOfExams.Length)
//            {
//                startTimesOfExams[index] = this.ExamsStartTime + TimeSpan.FromTicks((this.ExamsLength.Ticks + this.TimeBetweenExams.Ticks) * index);

//                //TODO: could be error if lunch time isn't in this time range, or just an error anywhere in it, or if exam lengths too long
//                // if need to account for lunch
//                if (this.LunchLength > this.timeBetweenExams)
//                {
//                    if (isLunchPast) // lunch period has passed
//                    {
//                        this.startTimesOfExams[index] += (this.LunchLength - this.TimeBetweenExams); // account for lunch time having replaced a break
//                    }
//                    else if (this.startTimesOfExams[index] + this.ExamsLength >= lowerLimitForLunch) // if exam ends in the range of lunch, lunch will immidiatly follow
//                    {
//                        isLunchPast = true;
//                    }
//                }
//                index++;
//            }
//        }


//        /// <summary>
//        /// Finds the index of the best fit for the block.
//        /// This is done by first creating a list of all
//        /// indexes in order of best fit then finding 
//        /// the first empty spot.
//        /// </summary>
//        /// <param name="startTime">average start time of block to schedule</param>
//        /// <returns>Index of best possible fit</returns>
//        public int FindBestTimeslotFit(TimeSpan startTime)
//        {

//            // create ordered list of times
//            // order by closeness to input start time, then by which one is later
//            // it's weird but easiest way
//            //TODO: clean up this
//            //TODO: look into reordering to find most valuable times more weighted by the enrollment numbers of that day.
//            List<TimeSpan> orderedPossibleTime = this.startTimesOfExams.ToList().OrderBy(c => (c - startTime)).ThenByDescending(c => c.Ticks).ToList();
//            List<int> orderedIndexesOfPossibleTime = new List<int>();
//            // sets up ordered list of all indexes 
//            foreach (TimeSpan time in orderedPossibleTime)
//            {
//                // for each index of time, add all indexes with that time to list
//                for (int i = GetIndexOfStartTime(time); i < this.blocks.Length; i += this.NumberOfTimeSlotsAvailablePerDay)
//                {
//                    orderedIndexesOfPossibleTime.Add(i);
//                }
//            }

//            //TODO: clean this up, should still work for the mean time
//            // ? could have exception for failing to return. that should never happen
//            // finds the first possible index in ordered list of best indexes that is empty
//            bool wasFound = false;
//            int indexOfIndex = 0;
//            while (indexOfIndex < orderedIndexesOfPossibleTime.Count && !wasFound)
//            {
//                if (this.Blocks.ElementAt(orderedIndexesOfPossibleTime[indexOfIndex]) == null) // if the spot at the index is empty
//                {
//                    wasFound = true;
//                }
//                else
//                {
//                    indexOfIndex++;
//                }
//            }

//            if (!wasFound)
//            {
//                throw new Exception("Best Fit not found in FindBestTimslotFit.");
//            }
//            else
//            {
//                return orderedIndexesOfPossibleTime[indexOfIndex];
//            }

//            //foreach (int index in orderedIndexesOfPossibleTime)
//            //{
//            //    if (this.Blocks.ElementAt(index) == null)
//            //        return index;
//            //}

//            ////TODO: this is bad
//            //Console.WriteLine("NOT GOOD: IN FindBestTimeslotFit");
//            //return 0;
//        }


//        //TODO: figure out cleaner way to do this. it needs to be converted to list and sorted but that loses indexes
//        /// <summary>
//        /// Finds the index of input start time in table of start times
//        /// </summary>
//        /// <param name="startTime">Start time whose index is desired.</param>
//        /// <returns></returns>
//        private int GetIndexOfStartTime(TimeSpan startTime)
//        {
//            bool wasFound = false;
//            int i = 0;
//            while (i < this.startTimesOfExams.Length && !wasFound)
//            {
//                if (startTimesOfExams[i] == startTime)
//                {
//                    wasFound = true;
//                }
//                else
//                {
//                    i++;
//                }
//            }
//            if (!wasFound)
//            {
//                throw new Exception("Invalid input start time. No valid start time was found.");
//            }
//            else
//            {
//                return i;
//            }

//            //////TODO: this is bad
//            ////Console.WriteLine("NOT GOOD: IN GetIndexOfStartTime");
//            ////return 0;

//        }

//        /// <summary>
//        /// Determines if blocks has any empty spaces
//        /// </summary>
//        /// <returns>Whether or not empty spaces exist</returns>
//        public bool IsFull()
//        {
//            bool isFull = true;
//            int i = 0;
//            while (i < blocks.Length && isFull)
//            {
//                if (blocks[i] == null)
//                {
//                    isFull = false;
//                }
//                i++;
//            }
//            return isFull;
//        }


//        /// <summary>
//        /// Gets the start time of exam block
//        /// </summary>
//        /// <param name="indexOfBlock"></param>
//        /// <returns></returns>
//        public TimeSpan GetStartTimeOfBlock(int indexOfBlock)
//        {
//            TimeSpan startTime = this.examsStartTime;
//            indexOfBlock %= this.NumberOfTimeSlotsAvailablePerDay;

//            for (int i = 0; i < indexOfBlock; i++)
//            {
//                startTime += this.ExamsLength + this.TimeBetweenExams;
//            }

//            return startTime;
//        }


//        /// <summary>
//        /// Placeholder
//        /// </summary>
//        /// <param name="groupedClasses"></param>
//        /// <returns></returns>
//        public void ScheduleBlocks(List<Block> groupedClasses)
//        {
//            this.blocks = new Block[this.numberOfTimeSlotsAvailable];
//            this.leftoverBlocks = groupedClasses.OrderByDescending(c => c.Enrollment).ToList();
//            while (this.leftoverBlocks.Count > 0 && !this.IsFull())
//            {
//                int index = this.FindBestTimeslotFit(this.leftoverBlocks[0].WeightedAverageStartTime);
//                this.blocks[index] = leftoverBlocks[0];
//                this.leftoverBlocks.RemoveAt(0);
//            }
//        }



//        //TODO: move into constructors
//        /// <summary>
//        /// Calculates number of timeslots available and sets it.
//        /// </summary>
//        private void SetNumberOfTimeSlotsAvailable()
//        {
//            TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go // TODO: maybe rewrite

//            TimeSpan lengthOfExamDay = latestTime - this.ExamsStartTime; // Figure out how much time available for exams

//            // if the lunch time is longer than the break time, account for it and the extra break time it will give you
//            if (this.LunchLength > this.TimeBetweenExams)
//            {
//                lengthOfExamDay -= (this.LunchLength - this.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
//            }

//            TimeSpan examFootprint = this.ExamsLength + this.TimeBetweenExams;

//            int numberOfExams = 0;
//            // TODO: bug if exam break is too big
//            while ((lengthOfExamDay - this.ExamsLength) >= TimeSpan.Zero)
//            {
//                lengthOfExamDay -= this.ExamsLength;
//                numberOfExams++;

//                // checks if a break is needed due to their being room for another exam after a break
//                if ((lengthOfExamDay - (this.TimeBetweenExams + this.ExamsLength) >= TimeSpan.Zero))
//                {
//                    lengthOfExamDay -= this.TimeBetweenExams;
//                }
//            }
//            this.numberOfTimeSlotsAvailablePerDay = numberOfExams;

//            this.numberOfTimeSlotsAvailable = this.NumberOfTimeSlotsAvailablePerDay * this.NumberOfDays;
//        }


//        //TODO: Needs work
//        // ? gut these display methods before shipping
//        /// <summary>
//        /// Displays all information stored in a Schedule instance with formatting.
//        /// </summary>
//        public void Display()
//        {
//            Console.WriteLine("\n*******************************");
//            Console.WriteLine("DISPLAYING SCHEDULE INFORMATION");
//            Console.WriteLine("*******************************");
//            Console.WriteLine("Number of Days: {0}", this.NumberOfDays);
//            Console.WriteLine("Start Time for Exams: {0}", this.ExamsStartTime);
//            Console.WriteLine("Length of Exams: {0}", this.ExamsLength);
//            Console.WriteLine("Time Between Exams: {0}", this.TimeBetweenExams);
//            Console.WriteLine("Length of Lunch Time: {0}", this.LunchLength);
//            Console.WriteLine("Number of Timeslots Per Day: {0}", this.NumberOfTimeSlotsAvailablePerDay);
//            Console.WriteLine("Number of Timeslots: {0}", this.NumberOfTimeSlotsAvailable);
//            Console.WriteLine("*******************************\n");
//        }


//        /// <summary>
//        /// Displays all blocks in schedule
//        /// </summary>
//        public void DisplayBlocks()
//        {
//            Console.WriteLine("\n***************************************");
//            Console.WriteLine("DISPLAYING SCHEDULED BLOCKS INFORMATION");
//            Console.WriteLine("***************************************");
//            for (int i = 0; i < this.blocks.Length; i++)
//            {
//                if (blocks[i] != null)
//                {

//                    Console.WriteLine("**************");
//                    Console.WriteLine("Block");
//                    Console.WriteLine("**************");
//                    Console.WriteLine("Block start time: {0}", this.startTimesOfExams[i % this.NumberOfTimeSlotsAvailablePerDay]);
//                    blocks[i].Display();
//                    Console.WriteLine("\nClasses In Block");
//                    Console.WriteLine("********");

//                    blocks[i].DisplayAllClasses();
//                    Console.WriteLine();

//                }
//                else
//                {
//                    Console.WriteLine("NOT GOOD: IN DisplayBlocks");
//                }

//            }

//            Console.WriteLine("\n*******************************************");
//            Console.WriteLine("DISPLAYING NON SCHEDULED BLOCKS INFORMATION");
//            Console.WriteLine("*******************************************");
//            foreach (Block block in this.leftoverBlocks)
//            {
//                Console.WriteLine("**************");
//                Console.WriteLine("Block");
//                Console.WriteLine("**************");
//                block.Display();
//                Console.WriteLine("\nClasses In Block");
//                Console.WriteLine("********");
//                block.DisplayAllClasses();
//                Console.WriteLine();
//            }
//        }


//        //TODO: Make bool to see if it's read
//        //TODO: This might need to be moved
//        //TODO: Catch exception that file couldn't be opened?
//        /// <summary>
//        /// Reads in the constraints file and initializes a static schedule
//        /// </summary>
//        /// <param name="inFileName"></param>
//        public void readInputConstraintsFile(string inFileName)
//        {
//            FileStream inFile = File.OpenRead(@inFileName);
//            var reader = new StreamReader(inFile);

//            this.numberOfDays = Int32.Parse(reader.ReadLine());
//            this.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
//            this.examsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
//            this.timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
//            this.lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

//            this.SetNumberOfTimeSlotsAvailable();
//        }


//        /// <summary>
//        /// Reads in a data file and constructs list of all classes in the file. Does
//        /// not add classes in that fall into the criteria of ignorable classes.
//        /// </summary>
//        /// <param name="inFileName"></param>
//        public void readInputDataFile(string inFileName)
//        {

//            // Make boundaries of ignored classes more usable
//            TimeSpan ignoreClassLength = TimeSpan.ParseExact(Schedule.CLASS_LENGTH_TO_START_IGNORING, @"hhmm", CultureInfo.InvariantCulture); // can't declare TimeSpan as const so do this here
//            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(Schedule.HOUR_TO_BEGIN_IGNORE_CLASS, @"hhmm", CultureInfo.InvariantCulture);

//            // Initialize all classes
//            this.allClasses = new List<Class>();

//            FileStream inFile = File.OpenRead(@inFileName);
//            var reader = new StreamReader(inFile);

//            reader.ReadLine(); // skip description line

//            while (!reader.EndOfStream)
//            {
//                // TODO: possibly change var to string
//                var line = reader.ReadLine(); // reads in next line
//                var values = line.Split(','); // splits into days/times and enrollement
//                var daysAndTimes = values[0].Split(' '); // chops up the days and times to manageable sections

//                TimeSpan startTime = TimeSpan.ParseExact(daysAndTimes[1], @"hhmm", CultureInfo.InvariantCulture); // 1 postion is the start time, changes formated time to bw more usable 
//                TimeSpan endTime = TimeSpan.ParseExact(daysAndTimes[3], @"hhmm", CultureInfo.InvariantCulture); // 3 position is the end time, changes formated time to bw more usable 

//                List<DayOfWeek> days = new List<DayOfWeek>(); // days the class meets
//                foreach (char day in daysAndTimes[0].ToCharArray()) // changes days from string of chars to list of DayOfWeek type
//                {
//                    switch (day)
//                    {
//                        case 'M':
//                            days.Add(DayOfWeek.Monday);
//                            break;
//                        case 'T':
//                            days.Add(DayOfWeek.Tuesday);
//                            break;
//                        case 'W':
//                            days.Add(DayOfWeek.Wednesday);
//                            break;
//                        case 'R':
//                            days.Add(DayOfWeek.Thursday);
//                            break;
//                        case 'F':
//                            days.Add(DayOfWeek.Friday);
//                            break;
//                    }
//                }

//                int enrollment = Int32.Parse(values[1]); // enrollement values should be in 1 position

//                if ((startTime <= ignoreClassStartTime) && (endTime - startTime < ignoreClassLength)
//                    && (days.Count > 1) && (TimeSpan.Compare(endTime - startTime, TimeSpan.FromMinutes(50)) == 0))
//                {
//                    this.allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list
//                }
//            }
//        }


//        /// <summary>
//        /// Takes in data for the constraints required to design a schedule
//        /// </summary>
//        /// <param name="numberOfDay"></param>
//        /// <param name="examsStartTime"></param>
//        /// <param name="examsLength"></param>
//        /// <param name="timeBetweenExams"></param>
//        /// <param name="lunchLength"></param>
//        public void SetupScheduleConstraints(int numberOfDay, TimeSpan examsStartTime,
//                                              TimeSpan examsLength, TimeSpan timeBetweenExams,
//                                              TimeSpan lunchLength)
//        {
//            this.numberOfDays = numberOfDay;
//            this.examsStartTime = examsStartTime;
//            this.examsLength = examsLength;
//            this.timeBetweenExams = timeBetweenExams;
//            this.lunchLength = lunchLength;
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        public void SaveSchedule(string path)
//        {
//            FileStream stream = File.Create(path);
//            BinaryFormatter formatter = new BinaryFormatter();
//            formatter.Serialize(stream, this);
//            stream.Close();
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        public void SaveScheduleToXML(string path)
//        {
//            System.Xml.Serialization.XmlSerializer XML =
//                new System.Xml.Serialization.XmlSerializer(this.GetType());

//            StreamWriter writer = new StreamWriter(path);
//            XML.Serialize(writer, this);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        public void OpenScheduleFromXML()
//        {

//        }


//        //public void ScheduleLunch()
//        //{
//        //    if (this.LunchLength > this.TimeBetweenExams)
//        //    {

//        //        TimeSpan lowerLimitForLunch = TimeSpan.ParseExact(Schedule.LOWER_TIME_RANGE_FOR_LUNCH, @"hhmm", CultureInfo.InvariantCulture);
//        //        int index = 0;
//        //        bool isLunchFound = false;
//        //        while (index < this.startTimesOfExams.Length) // while we haven't gone through the whole schedule
//        //        {


//        //            index++;


//        //        }
//        //        this.startTimesOfExams
//        //    }
//        //}


//        #endregion


//        #region Overloaded Operators
//        //Overloaded Operators Here

//        #endregion
//    }
//    #endregion
//}