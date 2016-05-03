// TODO: need to figure out standard for usage of this."
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;  // allows times to be different pased on local TODO: may can be removed
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace FETP
{
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

        /// <summary>
        /// Lower limit for when lunch can start
        /// </summary>
        public const string LOWER_TIME_RANGE_FOR_LUNCH = "1100";

        /// <summary>
        /// Upper limit for when lunch can start
        /// TODO: May not currently be in use
        /// </summary>
        public const string UPPER_TIME_RANGE_FOR_LUNCH = "0100";

        /// <summary>
        /// Lower limit for number of exam days
        /// </summary>
        public const int MIN_NUMBER_OF_DAYS_FOR_EXAMS = 1;
        /// <summary>
        /// Upper limit for number of exam days
        /// </summary>
        public const int MAX_NUMBER_OF_DAYS_FOR_EXAMS = 7;

        /// <summary>
        /// Lower limit for exam start time
        /// </summary>
        public const string MIN_START_TIME = "0700";
        /// <summary>
        /// Upper limit for exam start time
        /// </summary>
        public const string MAX_START_TIME = "1600"; // TODO: figure these out

        /// <summary>
        /// Lower limit for exam length
        /// </summary>
        public const int MIN_EXAM_LENGTH_IN_MINUTES = 90;
        /// <summary>
        /// Upper limit for exam length
        /// </summary>
        public const int MAX_EXAM_LENGTH_IN_MINUTES = 120; // TODO: figure these out

        /// <summary>
        /// Lower limit for break time length
        /// </summary>
        public const int MIN_BREAK_TIME_IN_MINUTES = 10;
        /// <summary>
        /// Upper limit for break time length
        /// </summary>
        public const int MAX_BREAK_TIME_IN_MINUTES = 30;

        /// <summary>
        /// Lower limit for lunch length 
        /// </summary>
        public const int MIN_LUNCH_LENGTH_IN_MINUTES = 0;
        /// <summary>
        /// Upper limit for lunch length
        /// </summary>
        public const int MAX_LUNCH_LENGTH_IN_MINUTES = 60;

        /// <summary>
        /// Format of time in files
        /// </summary>
        public const string TIME_FORMAT_FROM_FILE = @"hhmm";
        /// <summary>
        /// Format of time from GUI
        /// </summary>
        public const string TIME_FORMAT_FROM_GUI = @"hh\:mm";
   

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

        /// <summary>
        /// List of blocks not scheduled
        /// </summary>
        private List<Block> leftoverBlocks;

        /// <summary>
        /// Array of start times on a day of exams
        /// </summary>
        private TimeSpan[] startTimesOfExams;

        /// <summary>
        /// Name of file for original constraints file
        /// </summary>
        private string originalConstraintsFilename;

        /// <summary>
        /// Name of file for original enrollment file
        /// </summary>
        private string originalEnrollmentFilename;

        /// <summary>
        /// Inputed start time
        /// </summary>
        private string originalStartTime;

        /// <summary>
        /// Inputed exam length
        /// </summary>
        private string originalExamLength;

        /// <summary>
        /// Inputed break length
        /// </summary>
        private string originalBreakLength;

        /// <summary>
        /// Inputed number of days
        /// </summary>
        private string originalNumberOfDays;

        /// <summary>
        /// INputed lunch length
        /// </summary>
        private string originalLunchLength;
        

        #endregion


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string OriginalConstraintsFilename
        {
            get
            {
                return this.originalConstraintsFilename;
            }
            set
            {
                //This is in here due to the constraints file name never
                //being passed to the schedule object.
                originalConstraintsFilename = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string OriginalEnrollmentFilename
        {
            get
            {
                return this.originalEnrollmentFilename;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalExamLength
        {
            get
            {
                return this.originalExamLength;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalStartTime
        {
            get
            {
                return this.originalStartTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalBreakLength
        {
            get
            {
                return this.originalBreakLength;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalLunchLength
        {
            get
            {
                return this.originalLunchLength;
            }
        }

        /// <summary>
        /// 
       /// </summary>
        public string OriginalNumberOfDays
        {
            get
            {
                return this.originalNumberOfDays;
            }
        }
        

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
        /// Gets all blocks in schedule ordered by enrollment
        /// </summary>
        public List<Block> AllBlocksOrderedByEnrollment
        {
            get
            {
                List<Block> orderedBlocks = new List<Block>();
                for (int i = 0; i < this.NumberOfTimeSlotsToBeUsed; i++)
                {
                    orderedBlocks.Add(this.Blocks[i]);
                }
                orderedBlocks.AddRange(this.LeftoverBlocks);
                return orderedBlocks.OrderByDescending(c => c.Enrollment).ToList();
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

        /// <summary>
        /// Getter property for number of timeslots used for scheduling
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

        #region Constructors
        /// <summary>
        /// </summary>
        public Schedule(string path) //TODO: possibly remove?
        {
        }


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
            //Persist original input data
            this.originalEnrollmentFilename = dataFileAddress;
            this.originalNumberOfDays = numberOfDays;
            this.originalStartTime = examsStartTime;
            this.originalExamLength = examsLength;
            this.originalBreakLength = timeBetweenExams;
            this.originalLunchLength = lunchLength;

            //Intial setup
            this.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);
            this.SetupClassDataFromFile(dataFileAddress);

            this.Build();

        }

        #endregion


        #region Setup Methods

        /// <summary>
        /// Builds out the rest of schedule
        /// </summary>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public void Build() //TODO: refine
        {
            this.SetNumberOfTimeSlotsAvailable(); //TODO: rewire what this function does // maybe

            // Group Classes
            this.leftoverBlocks = FETP_Controller.GroupClasses(this.AllClasses);

            this.SetupExamStartTimeTable();

            this.SetupNumberOfTimeSlotsNeeded();

            this.ScheduleBlocks(this.LeftoverBlocks);
        }

        /// <summary>
        /// Checks if less timeslots are needed than available. 
        /// If so, then sets up control variable to not consider extra slots when scheduling
        /// </summary>
        //Author: Benjamin Etheredge
        //Date: 4-29-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetupNumberOfTimeSlotsNeeded()
        {
            if (this.LeftoverBlocks == null) // TODO: work on error checking
            {
                throw new Exception("Classes have not been grouped yet");
            }
            else
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
        }

        /// <summary>
        /// Sets up data member examsStartTimes to be used for quickly finding the start time of an index
        /// </summary>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetupExamStartTimeTable()
        {
            // setup lower limir for lunch from constant for easier use
            TimeSpan lowerLimitForLunch = TimeSpan.ParseExact(Schedule.LOWER_TIME_RANGE_FOR_LUNCH, TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture);

            this.startTimesOfExams = new TimeSpan[this.NumberOfTimeSlotsAvailablePerDay]; // intialize start times table

            int index = 0;
            bool isLunchPast = false; // accounts for exams after lunch to allow lunch time to be added to their start times
            while (index < startTimesOfExams.Length)
            {
                startTimesOfExams[index] = this.ExamsStartTime + TimeSpan.FromTicks((this.ExamsLength.Ticks + this.TimeBetweenExams.Ticks) * index);

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

        /// <summary>
        /// Calculates number of timeslots available and sets it.
        /// </summary>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetNumberOfTimeSlotsAvailable()
        {
            TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture); // latest exams can go // TODO: maybe rewrite

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

        /// <summary>
        /// Reads in a data file and constructs list of all classes in the file. Does
        /// not add classes in that fall into the criteria of ignorable classes.
        /// </summary>
        /// <param name="inFileName"></param>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetupClassDataFromFile(string inFileName)
        {
            //Maintain the path to the original file used to generate schedule
            this.originalEnrollmentFilename = inFileName;

            // Make boundaries of ignored classes more usable
            TimeSpan ignoreClassLength = TimeSpan.ParseExact(Schedule.CLASS_LENGTH_TO_START_IGNORING, TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture); // can't declare TimeSpan as const so do this here
            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(Schedule.HOUR_TO_BEGIN_IGNORE_CLASS, TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture);

            // Initialize all classes
            this.allClasses = new List<Class>();

            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile);

            reader.ReadLine(); // skip description line

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine(); // reads in next line
                var values = line.Split(','); // splits into days/times and enrollement
                var daysAndTimes = values[0].Split(' '); // chops up the days and times to manageable sections

                TimeSpan startTime = TimeSpan.ParseExact(daysAndTimes[1], TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture); // 1 postion is the start time, changes formated time to bw more usable 
                TimeSpan endTime = TimeSpan.ParseExact(daysAndTimes[3], TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture); // 3 position is the end time, changes formated time to bw more usable 

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
        /// Reads in the constraints file and initializes a static schedule
        /// </summary>
        /// <param name="inFileName"></param>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetupScheduleConstraintsFromFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile); // TODO: maybe remove var to conform to standards

            //TODO: possibly implement TryParse or other form of error handling
            this.numberOfDays = Int32.Parse(reader.ReadLine());
            this.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), TIME_FORMAT_FROM_FILE, CultureInfo.InvariantCulture); //TODO: further investigate CultureInfo.InvariantCulture to be sure it's needed and doesn't break stuff
            this.examsLength = TimeSpan.FromMinutes(Double.Parse(reader.ReadLine()));
            this.timeBetweenExams = TimeSpan.FromMinutes(Double.Parse(reader.ReadLine())); //TODO: Test to make sure the from minutes functions with CultureInfo.InvariantCulture
            this.lunchLength = TimeSpan.FromMinutes(Double.Parse(reader.ReadLine()));

        }

        /// <summary>
        /// Sets up all constraints for scheduling
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <param name="examsStartTime"></param>
        /// <param name="examsLength"></param>
        /// <param name="timeBetweenExams"></param>
        /// <param name="lunchLength"></param>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void SetupScheduleConstraints(string numberOfDays, string examsStartTime,
                                              string examsLength, string timeBetweenExams,
                                              string lunchLength)
        {
            this.numberOfDays = Int32.Parse(numberOfDays);
            this.examsStartTime = TimeSpan.ParseExact(examsStartTime, TIME_FORMAT_FROM_GUI, CultureInfo.InvariantCulture); //TODO: further investigate CultureInfo.InvariantCulture to be sure it's needed and doesn't break stuff
            this.examsLength = TimeSpan.FromMinutes(Int32.Parse(examsLength));
            this.timeBetweenExams = TimeSpan.FromMinutes(Int32.Parse(timeBetweenExams)); //TODO: Test to make sure the from minutes functions with CultureInfo.InvariantCulture
            this.lunchLength = TimeSpan.FromMinutes(Int32.Parse(lunchLength));
        }

        #endregion


        #region Scheduling Functions


        //TODO: further investigate what makes the best time
        /// <summary>
        /// Finds the index of the best fit for the block.
        /// This is done by first creating a list of all
        /// indexes in order of best fit then finding 
        /// the first empty spot.
        /// </summary>
        /// <param name="startTime">average start time of block to schedule</param>
        /// <returns>Index of best possible fit</returns>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private int FindBestTimeslotFit(TimeSpan startTime)
        {

            // create ordered list of times
            // order by closeness to input start time, then by which one is later
            // it's weird but easiest way
            //TODO: clean up this
            //TODO: look into reordering to find most valuable times more weighted by the enrollment numbers of that day.
            List<TimeSpan> orderedPossibleTime = this.startTimesOfExams.ToList().OrderBy(c => (c - startTime).Duration()).ThenByDescending(c => c.Ticks).ToList(); // TODO: FURTHER TEST
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
        /// <returns>Index of inputed start time</returns>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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
        /// Gets the start time of exam block
        /// </summary>
        /// <param name="indexOfBlock"></param>
        /// <returns>Start time of inputed indexes block</returns>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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
        /// Determines if blocks has any empty spaces
        /// </summary>
        /// <returns>Whether or not empty spaces exist</returns>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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
        /// Schedules inputed grouped classes
        /// </summary>
        /// <param name="groupedClasses">Schedules inputed grouped classes into Schedule object</param>
        //Author: Benjamin Etheredge
        //Date: 3-30-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        private void ScheduleBlocks(List<Block> groupedClasses)
        {
            this.blocks = new Block[this.numberOfTimeSlotsAvailable]; // sets up Scheduled classes array
            this.leftoverBlocks = groupedClasses.OrderByDescending(c => c.Enrollment).ToList(); // orders all grouped classes by enrollment
            while (this.leftoverBlocks.Count > 0 && !this.IsFull())
            {
                int index = this.FindBestTimeslotFit(this.leftoverBlocks[0].WeightedAverageStartTime); // finds best empty slot to insert block
                this.blocks[index] = leftoverBlocks[0]; // inserts block
                this.leftoverBlocks.RemoveAt(0); // removes block from list
            }
        }


        #endregion


        #region Schedule Functionality Tools

        /// <summary>
        /// Swaps the blocks at the inputed indexs //TODO: indices? indexes? 
        /// </summary>
        /// <param name="index1">Index of the first block to swap</param>
        /// <param name="index2">Index of the second block to swap</param>
        /// <returns></returns>
        public void SwitchBlocks(int index1, int index2)
        {
            Block temp = this.Blocks[index1];
            this.Blocks[index1] = this.Blocks[index2];
            this.Blocks[index2] = temp;
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

        /// <summary>
        /// 
        /// </summary>
        public void ExportTextSchedule(string path, string[] text)
        {
            File.WriteAllLines(path, text);
        }

        #endregion

        #endregion


        #region Overloaded Operators
        //Overloaded Operators Here

        #endregion
    }
}


