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
    /**************************************************************************\
    Class: Schedule (Genetic Algorithm Constraints)
    Description: The Constraints class contains all of the methods needed for
                 checking if a chromosome is meeting soft and hard constraints. 
    // TODO: if a function can modify the internal variables of class, it should be in that class
    \**************************************************************************/
    public class Schedule
    {
        /**************************************************************************\
        Class: Schedule 
        Section: Utilities
        \**************************************************************************/
        //Utility Data Members Here


        /**************************************************************************\
        Utility Method: Example 
        Description: This is an example header
        TODO: Remove if a utility method is added
        \**************************************************************************/

        //End Utilities Section


        /**************************************************************************\
        Class: Block 
        Section: Data Constants 
        \**************************************************************************/
        private const string CLASS_LENGTH_TO_START_IGNORING = "0120"; // TODO: clean these up
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        //End Data Constants Section


        /**************************************************************************\
        Class: Schedule
        Section: Data Members
        \**************************************************************************/
        //Static Members
        private static int numberOfDays;
        private static TimeSpan examsStartTime;
        private static TimeSpan examsLength;
        private static TimeSpan timeBetweenExams;
        private static TimeSpan lunchLength;
        private static List<Class> allClasses;
        private static int numberOfTimeSlotsAvailable;

        //Non-static Members
        private List<Block> blocks;

        //End Data Members Section


        /**************************************************************************\
        Class: Schedule 
        Sections: Properties
        TODO: Let me know if headers for properties are too excessive (VR)
        \**************************************************************************/
        /**************************************************************************\
        Property: Blocks 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public List<Block> Blocks
        {
            get
            {
                return this.blocks;
            }
            //set
            //{
            //    this.blocks = value;
            //}
        }


        /**************************************************************************\
        Property: NumberOfDays 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static int NumberOfDays
        {
            get { return numberOfDays; }
            //set { Schedule.numberOfDays = value; }
        }


        /**************************************************************************\
        Property: ExamsStartTime 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static TimeSpan ExamsStartTime
        {
            get { return examsStartTime; }
            //set { Schedule.examsStartTime = value; }
        }


        /**************************************************************************\
        Property: ExamsLength 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static TimeSpan ExamsLength
        {
            get { return examsLength; }
            //set { Schedule.examsLength = value; }
        }


        /**************************************************************************\
        Property: TimeBetweenExams 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static TimeSpan TimeBetweenExams
        {
            get { return timeBetweenExams; }
            //set { Schedule.timeBetweenExams = value; }
        }


        /**************************************************************************\
        Property: LunchLength 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static TimeSpan LunchLength
        {
            get { return lunchLength; }
            //set { this.lunchLength = value; }
        }


        /**************************************************************************\
        Property: AllClasses 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static List<Class> AllClasses
        {
            get
            {
                return Schedule.allClasses;
            }
        }


        /**************************************************************************\
        Property: NumberOfTimeSlotsAvailable 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public static int NumberOfTimeSlotsAvailable
        {
            get
            {
                return Schedule.numberOfTimeSlotsAvailable;
            }
            
        }


        /**************************************************************************\
        Property: NumberOfTimeSlotsAvailablePerDay
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
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


        /**************************************************************************\
        Property: FitnessScore 
        Description: !?!?
        TODO: Add a description
        \**************************************************************************/
        public double FitnessScore
        {
            get
            {
                double fitnessScore = 0;
                foreach(Block block in this.blocks)
                {
                    fitnessScore += block.FitnessScore;
                }
                // more weighting stuff here
                return fitnessScore;
            }
        } // TODO: possible optimation, if private var is null, set it. then return. TODO: maybe add in Parallel foreach loop to do that if fitness score is heavy to calculate?

        //End Properties Section


        /**************************************************************************\
        Class: Schedule
        Section: Methods
        \**************************************************************************/
        /**************************************************************************\
        Method: Default Constructor
        Description: Creates a random schedule off the incoming list of classes
        TODO: possibly make classes static. it would make it faster
        \**************************************************************************/
        public Schedule() 
        {
            SetUpBlocks();

            this.PigeonHoleClasses(Schedule.AllClasses.OrderBy(c => GA_Controller.GetRandomInt()).ToList()); // sort in randomly order classes
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
        Constructor: Schedule Combining Constructor
        Description: Creates a schedule off parent schedules. I alternates 
                     taking blocks from each parent.
        Note: for two kids, call constructor twice with parent 
              schedules in different spots 
        \**************************************************************************/
        public Schedule(Schedule schedule1, Schedule schedule2)
        {
            SetUpBlocks();

            // int blockCount = Schedule.NumberOfTimeSlotsAvailable;
            for (int i = 0; i < Schedule.NumberOfTimeSlotsAvailable; i++) // maybe swap just whole halves
            {
                if (i % 2 == 0)
                {
                    this.blocks[i] = schedule1.Blocks[i];
                }
                else
                {
                    this.blocks[i] = schedule2.Blocks[i];
                }
            }


            this.AttemptMutate(); // TODO: break up for cohesion

            // TODO: don't need randomness ?
        }


        /**************************************************************************\
        Method: SetNumberOfTimeSlotsAvailable
        Description: Calculates number of timeslots available and sets it.
        \**************************************************************************/
        private static void SetNumberOfTimeSlotsAvailable()
        {
            Schedule.numberOfTimeSlotsAvailable = Schedule.NumberOfTimeSlotsAvailablePerDay * Schedule.NumberOfDays;
        }


        /**************************************************************************\
        Method: WillMutate
        Description: Deteremines whether a mutation should occur
        \**************************************************************************/
        private bool WillMutate()
        {
            bool willItMutate = false;
            float randFloatBetween01 = FETP_Controller.RandomFloatBetween01();

            if (randFloatBetween01 < GA_Controller.MUTATION_RATE)
            {
                willItMutate = true;
            }

            return willItMutate;
        }


        /**************************************************************************\
        Method: Mutate
        Description: Mutates a scheudle
                     currently picks two random blocks, picks midpoints in those
                     blocks classes, then cuts the classes at that point, then
                     swaps them
        \**************************************************************************/
        public void Mutate()
        {
            Random rnd = new Random();

            // select two random blocks to combine
            int blockIndex1 = rnd.Next(0, this.blocks.Count); // TODO: this makes it possible to not mutate with 0? maybe. over weigting chance to not mutate?
            int blockIndex2 = rnd.Next(0, this.blocks.Count);

            // select a mid point in classes to swap from
            int midPointInClasses1 = rnd.Next(0, this.blocks[blockIndex1].ClassesInBlock.Count);
            int midPointInClasses2 = rnd.Next(0, this.blocks[blockIndex2].ClassesInBlock.Count);

            // swap parts of classes
            List<Class> tempClasses1 = blocks[blockIndex1].ClassesInBlock.GetRange(0, blocks[blockIndex1].ClassesInBlock.Count); // gets the objects from the beginning to index

            // TODO: make more readable
            blocks[blockIndex1].ClassesInBlock.RemoveRange(0, blocks[blockIndex1].ClassesInBlock.Count);
            blocks[blockIndex1].ClassesInBlock.AddRange(blocks[blockIndex2].ClassesInBlock.GetRange(0, blocks[blockIndex2].ClassesInBlock.Count)); // adds the range from the second class to block 1 //TODO: maybe not right to get front half from both ???
            blocks[blockIndex2].ClassesInBlock.RemoveRange(0, blocks[blockIndex2].ClassesInBlock.Count);
            blocks[blockIndex2].ClassesInBlock.AddRange(tempClasses1);
        }


        /**************************************************************************\
        Method: AttemptMutate
        Description: Attempts to mutate a scheudle. Runs a test to see if it
                     will mutate. If so, it mutates the schedule.                
        \**************************************************************************/
        public bool AttemptMutate()
        {
            bool didItMutate = false;
            if (this.WillMutate()) 
            {
                this.Mutate();
                didItMutate = true;
            }
            return didItMutate;
        }


        /**************************************************************************\
        Method: Display
        Description: Displays all informations stored in Schedule instance
                     with formatting.
        TODO: needs finishing
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


        /**************************************************************************\
        Method: readInputConstraintsFile
        Description: Reads in constraints file and intializes static schedule
        TODO: maybe make bool to see if it is read
        TODO: this might need to be moved
        TODO: catch exception that file couldn't be opened?
              data members
        \**************************************************************************/
        public static void readInputConstraintsFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile); // TODO: 

            Schedule.numberOfDays = Int32.Parse(reader.ReadLine());
            Schedule.examsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.examsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            Schedule.lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

            Schedule.SetNumberOfTimeSlotsAvailable();
        }


        /**************************************************************************\
        Method: readInputDataFile
        Description: Reads in data file and constructs list of all classes
                     in the file. Does not add classes in that fall into the
                     criteria of ignorable classes
        TODO: maybe make bool to see if it is read
        TODO: this might need to be moved
        TODO: break up for coehesion and for easier use of manual input
              takes in an open data file and returns a list of all the classes
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
                // TODO: possibly change var to string
                var line = reader.ReadLine(); // reads in next line
                var values = line.Split(','); // splits into days/times and enrollement
                var daysAndTimes = values[0].Split(' '); // chops up the days and times to manageable sections

                TimeSpan startTime = TimeSpan.ParseExact(daysAndTimes[1], @"hhmm", CultureInfo.InvariantCulture); // 1 postion is the start time, changes formated time to bw more usable 
                TimeSpan endTime = TimeSpan.ParseExact(daysAndTimes[3], @"hhmm", CultureInfo.InvariantCulture); // 3 position is the end time, changes formated time to bw more usable 

                // Checks if class should not be ignored before continuing execution
                if ((startTime < ignoreClassStartTime) && (endTime - startTime < ignoreClassLength))
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

                    Schedule.allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list
                }
            }
        }


        /**************************************************************************\
        Method: PigeonHoleClasses
        Description: Puts classes in blocks sequentially. Used in random schedule
                     creation. Takes in a list of classes and coalesces them into a
                     list of blocks of classes.
        // TODO: only runs during one generation. maybe move ?
        \**************************************************************************/
        private void PigeonHoleClasses(List<Class> classes) // TODO: I LOVE PIGEON HOLE
        {
            
            foreach (Class cl in classes)
            {
                int i = GA_Controller.GetRandomInt(this.Blocks.Count);

                // TODO: maybe check for empty blocks ?
                this.blocks[i].addClass(cl);

                //if (i < Schedule.NumberOfTimeSlotsAvailable - 1) // TODO: clean up
                //{
                //    i++;
                //}
                //else
                //{
                //    i = 0;
                //}
            }
        }

        //End Methods Section


        /**************************************************************************\
        Class: Schedule 
        Section: Overloaded Operators 
        \**************************************************************************/
        /**************************************************************************\
        Operator: ==
        Description: This is an example 
        \**************************************************************************/

        //End Overloaded Operators Section
    }
}
