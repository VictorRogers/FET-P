using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // ?? some of these aren't needed

using System.Globalization;  // allows times to be different pased on local ? may can be removed
using System.Diagnostics;


namespace FETP
{

    /**************************************************************************\
    Class: Schedule (Genetic Algorithm Constraints)
    Description: The Constraints class contains all of the methods needed for
                 checking if a chromosome is meeting soft and hard constraints. 
    \**************************************************************************/
    public class Schedule
    {

        /**************************************************************************\
        Schedule - Constant Data Members 
        \**************************************************************************/
        public const string TIME_EXAMS_MUST_END_BY = "1700";


        /**************************************************************************\
        Schedule - Static Data Members 
        \**************************************************************************/
        private static int numberOfDays;
        private static TimeSpan examsStartTime;
        private static TimeSpan examsLength;
        private static TimeSpan timeBetweenExams;
        private static TimeSpan lunchLength;
        // could make List of all classes static


        /**************************************************************************\
        Schedule - Data Members 
        \**************************************************************************/
        private List<Block> blocks; 
        

        /**************************************************************************\
        Schedule - Properties 
        \**************************************************************************/
        public List<Block> Blocks
        {
            get
            {
                return this.blocks;
            }
            set
            {
                this.blocks = value;
            }
        }
        public static int NumberOfDays
        {
            get { return numberOfDays; }
            set { this.numberOfDays = value; }
        }
        public static TimeSpan ExamsStartTime
        {
            get { return examsStartTime; }
            set { this.examsStartTime = value; }
        }
        public static TimeSpan ExamsLength
        {
            get { return examsLength; }
            set { this.examsLength = value; }
        }
        public static TimeSpan TimeBetweenExams
        {
            get { return timeBetweenExams; }
            set { this.timeBetweenExams = value; }
        }
        public static TimeSpan LunchLength
        {
            get { return lunchLength; }
            set { this.lunchLength = value; }
        }
        public static int NumberOfTimeSlotsAvailable
        {
            get
            {
                return this.NumberOfTimeSlotsAvailablePerDay * this.numberOfDays;
            }
            
        }
        public static int NumberOfTimeSlotsAvailablePerDay
        {
            get
            {
                TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go // ? maybe rewrite

                TimeSpan lengthOfExamDay = latestTime - this.ExamsStartTime; // Figure out how much time available for exams

                // if the lunch time is longer than the break time, account for it and the extra break time it will give you
                if (this.LunchLength > this.TimeBetweenExams)
                {
                    lengthOfExamDay -= (this.LunchLength - this.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
                }

                TimeSpan examFootprint = this.ExamsLength + this.TimeBetweenExams;

                int numberOfExams = 0;
                // ? bug if exam break is too big
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
        public int FitnessScore
        {
            get
            {
                int fitnessScore = 0;
                foreach(Block block in this.blocks)
                {
                    fitnessScore += block.FitnessScore;
                }
                // more weighting stuff here
                return fitnessScore;
            }
        }


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

        // creates a random schedule
        // maybe makes list of all classes static
        // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? here victor 1/4
        /**************************************************************************\
        Constructor: Random Constructor
        Description: Creates a random schedule off the incoming list of classes
        \**************************************************************************/
        public Schedule(List<Class> classes) // ? possibly make classes static. it would make it faster
        {
            
            Random rand = new Random();
            classes = classes.OrderBy(c => rand.Next()).ToList(); // randomly arrange classes ? i think

            this.blocks = FETP_Controller.coalesceClassesTogether(classes);
        }

        // ? call twice for keeeeds. swap postions of parents
        // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? here victor 2/4
        /**************************************************************************\
        Constructor: Schedule Combining Constructor
        Description: Creates a schedule off parent schedules. I alternates 
                     taking blocks from each parent.
        Note: for two kids, call constructor twice with parent 
              schedules in different spots 
        \**************************************************************************/
        public Schedule(Schedule schedule1, Schedule schedule)
        {

            // int blockCount = Schedule.NumberOfTimeSlotsAvailable;
            for(int i = 0; i < Schedule.NumberOfTimeSlotsAvailable; i++) // maybe swap just whole halves
            {
                if(i % 2 == 0)
                {
                    this.blocks[i] = schedule1.Blocks[i];
                }
                else
                {
                    this.blocks[i] = schedule2.Blocks[i];
                }
            }

            Mutate(); // ? break up for cohesion

            // don't need randomness ?
        }

        // ?
        // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? here victor 3/4
        /**************************************************************************\
        Method: WillMutate
        Description: Deteremines whether a mutation should occur
        \**************************************************************************/
        private bool WillMutate()
        {
            // ? victor rewrite
            Random rnd = new Random();
            return (rnd.Next(0, 20) == 1);
        }


        // ?
        // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? here victor 4/4
        /**************************************************************************\
        Method: Mutate
        Description: Mutates a scheudle
                     currently picks two random blocks, picks midpoints in those
                     blocks classes, then cuts the classes at that point, then
                     swaps them
        \**************************************************************************/
        public bool Mutate()
        {
            if (WillMutate()) // break up for coehesion?
            {
                Random rnd = new Random();

                // select two random blocks to combine
                int blockIndex1 = rnd.Next(0, this.blocks.Count); // ? this makes it possible to not mutate with 0? maybe. over weigting chance to not mutate?
                int blockIndex2 = rnd.Next(0, this.blocks.Count);

                // select a mid point in classes to swap from
                int midPointInClasses1 = rnd.Next(0, this.blocks[blockIndex1].ClassesInBlock.Count);
                int midPointInClasses2 = rnd.Next(0, this.blocks[blockIndex2].ClassesInBlock.Count);

                // swap parts of classes
                List<Class> tempClasses1 = blocks[blockIndex1].ClassesInBlock.GetRange(0, blocks[blockIndex1].ClassesInBlock.Count); // gets the objects from the beginning to index

                // ? make more readable
                blocks[blockIndex1].ClassesInBlock.RemoveRange(0, blocks[blockIndex1].ClassesInBlock.Count);
                blocks[blockIndex1].ClassesInBlock.AddRange(blocks[blockIndex2].ClassesInBlock.GetRange(0, blocks[blockIndex2].ClassesInBlock.Count)); // adds the range from the second class to block 1 // maybe not right to get front half from both ???
                blocks[blockIndex2].ClassesInBlock.RemoveRange(0, blocks[blockIndex2].ClassesInBlock.Count);
                blocks[blockIndex2].ClassesInBlock.AddRange(tempClasses1);

                return true;
            }
            else return false;
        }


        // ? i don't think i'll ever need this 
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        //public Schedule(Schedule inSchedule = null, List<Block> inBlocks = null)
        //{
        //    if (inSchedule != null)
        //    {
        //        this.numberOfDays = inSchedule.NumberOfDays;
        //        this.examsStartTime = inSchedule.ExamsStartTime;
        //        this.examsLength = inSchedule.ExamsStartTime;
        //        this.timeBetweenExams = inSchedule.TimeBetweenExams;
        //        this.lunchLength = inSchedule.LunchLength;
        //    }
        //    // ? could assign inSchedules days to it
        //    this.blocks = inBlocks;
        //}

        // ?
        // needs finishing
        /**************************************************************************\
        Method: Display
        Description: Displays all informations stored in Schedule instance
                     with formatting.
        \**************************************************************************/
        public void Display()
        {
            Console.WriteLine("Number of Days: {0}", numberOfDays);
            Console.WriteLine("Start Time for Exams: {0}", examsStartTime);
            Console.WriteLine("Length of Exams: {0}", examsLength);
            Console.WriteLine("Time Between Exams: {0}", timeBetweenExams);
            Console.WriteLine("Length of Lunch Time: {0}", lunchLength);
        }
        

    } // end class Schedule
}
