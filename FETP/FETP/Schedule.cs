using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // ?? some of these aren't needed

using System.Diagnostics;


namespace FETP
{
    public class Schedule
    {
        public List<Block> blocks; // ? lazy
        protected int numberOfDays;
        protected TimeSpan examsStartTime;
        protected TimeSpan examsLength;
        protected TimeSpan timeBetweenExams;
        //public TimeSpan lunchTime; // ? lazy
        protected TimeSpan lunchLength;
        protected int fitnessScore;

        // Properties / Accessors and Mutators
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

        //public int FitnessScore
        //{
        //    get
        //    {
        //        {

        //        }
        //    }
        //}


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
