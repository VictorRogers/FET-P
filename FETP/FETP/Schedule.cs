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
        public const string CLASS_LENGTH_TO_START_IGNORING = "0245"; // const is always static ?
        public const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";
        public const string TIME_EXAMS_MUST_END_BY = "1700";

        /**************************************************************************\
        Schedule - Data Members 
        \**************************************************************************/
        private List<Block> blocks; 
        private int numberOfDays;
        private TimeSpan examsStartTime;
        private TimeSpan examsLength;
        private TimeSpan timeBetweenExams;
        //public TimeSpan lunchTime; // ? lazy
        private TimeSpan lunchLength;
        //private int fitnessScore;


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
        public int NumberOfDays
        {
            get { return numberOfDays; }
            set { this.numberOfDays = value; }
        }
        public TimeSpan ExamsStartTime
        {
            get { return examsStartTime; }
            set { this.examsStartTime = value; }
        }
        public TimeSpan ExamsLength
        {
            get { return examsLength; }
            set { this.examsLength = value; }
        }
        public TimeSpan TimeBetweenExams
        {
            get { return timeBetweenExams; }
            set { this.timeBetweenExams = value; }
        }
        public TimeSpan LunchLength
        {
            get { return lunchLength; }
            set { this.lunchLength = value; }
        }
        public int NumberOfTimeSlotsAvailable
        {
            get
            {
                return this.NumberOfTimeSlotsAvailablePerDay * this.numberOfDays;
            }
            
        }
        public int NumberOfTimeSlotsAvailablePerDay
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
        public Schedule(int inNumberOfDays, TimeSpan inExamsStartTime, TimeSpan inExamsLength, TimeSpan inTimeBetweenExams, TimeSpan inLunchLength, List<Block> inDays = null)
        {
            this.numberOfDays = inNumberOfDays;
            this.examsStartTime = inExamsStartTime;
            this.examsLength = inExamsLength;
            this.timeBetweenExams = inTimeBetweenExams;
            this.lunchLength = inLunchLength;

            this.blocks = inDays;
        }

        // ? i don't think i'll ever need this 
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        public Schedule(Schedule inSchedule = null, List<Block> inBlocks = null)
        {
            if (inSchedule != null)
            {
                this.numberOfDays = inSchedule.NumberOfDays;
                this.examsStartTime = inSchedule.ExamsStartTime;
                this.examsLength = inSchedule.ExamsStartTime;
                this.timeBetweenExams = inSchedule.TimeBetweenExams;
                this.lunchLength = inSchedule.LunchLength;
            }
            // ? could assign inSchedules days to it
            this.blocks = inBlocks;
        }

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
