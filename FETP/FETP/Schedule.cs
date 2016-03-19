using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public TimeSpan lunchTime; // ? lazy
        protected TimeSpan lunchLength;

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

        // public void addDay()

        /*
   // fills in days with blank timeslots
   public void fillInDays()
    {
        List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

        for (int i = 0; i < this.numberOfDays; i++)
        {
            Day newDay = new Day(examsStartTime);
            TimeSpan examCurrentTime = this.examsStartTime;
            for (int j = 0; j < FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(this); j++)
            {
                newDay.blocks.Add(new Timeslot(examCurrentTime, examCurrentTime + examsLength));
                // ? check if lunch and account for it here
                examCurrentTime += examsLength + timeBetweenExams;
            }
        }
    }
    */

        // ?
        // needs finishing
        public void Display()
        {
            Console.WriteLine("Number of Days: {0}", numberOfDays);
            Console.WriteLine("Start Time for Exams: {0}", examsStartTime);
            Console.WriteLine("Length of Exams: {0}", examsLength);
            Console.WriteLine("Time Between Exams: {0}", timeBetweenExams);
            Console.WriteLine("Length of Lunch Time: {0}", lunchLength);
            //if (days != null)
            //{
            // display days
            // ? may not need
            //
        }






        // takes in a list of classes 
        // returns a new list of groups of classes
        // can be used to find a grouping size to beat
        public static List<Block> coalesceClassesTogether(List<Class> classes)
        {
            List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

            foreach (Class cl in classes)
            {
                classesToBeGrouped = groupClass(classesToBeGrouped, cl); // ? clean this up

            }

            return classesToBeGrouped;


        }









        // ? let's get dirty

        public volatile int smallestGroupSizeFound;
        public volatile List<List<Block>> smallestGroupsFound = new List<List<Block>>(); 

        // takes in a list of class groups and a class
        // returns a new list of class groups with class inserted into first possible group
        public static List<Block> groupClass(List<Block> blocks, Class inClass)
        {
            bool isInserted = false;
            int i = 0;

            while (i < blocks.Count && !(isInserted = blocks[i++].addClass(inClass))) ; // ? i <3 my while loops that terminate in a semicolen

            if (!isInserted)
                blocks.Add(new Block(inClass));
            return blocks;
        }

        // finds the smallest grouping
        public void groupClasses(List<Class> classes)
        {
            List<Block> blocks = new List<Block>();
            //groupingHelper(blocks, classes);
            //helpMe(blocks, classes);
            //Console.WriteLine("{0}", findSmallestGroup(blocks, classes));
            Console.WriteLine("{0}", findNextSmallestGroup(blocks, classes, this.smallestGroupSizeFound));
        }



        // ?????
        public static List<Class> removeClass(List<Class> classes, Class inClass)
        {
            int i = 0;
            while (i < classes.Count)
            {
                if (classes[i] == inClass)
                {
                    classes.RemoveAt(i);
                    return classes;
                }
            }
            return classes;
        }





        //// Recursive Helper
        //protected List<Block> groupingHelper(List<Block> blocks, List<Class> classes)
        //{

        //    if (classes.Count == 0)
        //    {
        //        return blocks;
        //    }
        //    else
        //    {
        //        //List<Block> newBlocks = blocks;

        //        foreach (Class cl in classes)
        //        {
        //            blocks = groupClass(blocks, cl);
        //        }

        //        if (blocks.Count <= this.smallestGroupSizeFound)
        //        {
        //            this.smallestGroupSizeFound = blocks.Count;
        //            this.smallestGroupsFound.Add(blocks);
        //        }

        //        foreach (Class cl in classes)
        //        {
        //            List<Block> newBlocks = blocks;
        //            List<Class> newClasses = classes;
        //            newClasses.Remove(cl);
        //            newBlocks = groupClass(newBlocks, cl);

        //            newBlocks = groupingHelper(newBlocks, classes);
        //            if(newBlocks.Count < this.smallestGroupSizeFound)
        //            {
        //                this.smallestGroupSizeFound = newBlocks.Count;
        //            }
        //        }
        //        return groupingHelper
        //    }
        //}

        //public static async Task findSmallest(List<Block>Blocks, List<Class> classes, int smallestThusFar)
        //{

        //}

        //public List<Thread> threads = new List<Thread>();

        //// Recursive Helper
        //// simulates all possible groups and only saves the smallest
        //protected void groupingHelper(List<Block> blocks, List<Class> classes, int smallestThusFar)
        //{

        //    if (classes.Count != 0 && blocks.Count < smallestThusFar)
        //    {
        //        //List<Block> newBlocks = blocks;
        //        int i = 0;
        //        while (i < classes.Count)
        //        {
        //            List<Block> newBlocks = new List<Block>(this.groupClass(blocks, classes[i]));
        //            List<Class> newClasses = new List<Class>(classes);
        //            newClasses.RemoveAt(i);

        //            thread.Add(new Thread((groupingHelper(newBlocks, newClasses, smallestThusFar));

        //            i++;
        //        }


        //        //foreach (Class cl in classes)
        //        //{
        //        //    List<Block> newBlocks = this.groupClass(blocks, cl);
        //        //    List<Class> newClasses = classes;
        //        //    Console.WriteLine("removing class");
        //        //    newClasses.Remove(cl);
        //        //    Console.WriteLine("removed class");
        //        //    groupingHelper(newBlocks, newClasses);

        //        //}
        //    }
        //    else if (blocks.Count <= this.smallestGroupSizeFound)
        //    {
        //        this.smallestGroupsFound.Add(blocks);
        //    }
        //}

        

        
        //public async Task<> helpMe(List<Block> blocks, List<Class> classes)
        //{
        //    await groupingHelper(blocks, classes);
        //}


        // Recursive Helper
        // simulates all possible groups and only saves the smallest
        //protected async void groupingHelper(List<Block> blocks, List<Class> classes)
        //{

        //    if (classes.Count != 0 && blocks.Count < this.smallestGroupSizeFound)
        //    {
        //        //List<Block> newBlocks = blocks;
        //        int i = 0;
        //        while (i < classes.Count)
        //        {
        //            List<Block> newBlocks = new List<Block>(Schedule.groupClass(blocks, classes[i]));
        //            List<Class> newClasses = new List<Class>(classes);
        //            newClasses.RemoveAt(i);

        //            groupingHelper(newBlocks, newClasses);

        //            i++;
        //        }


        //        //foreach (Class cl in classes)
        //        //{
        //        //    List<Block> newBlocks = this.groupClass(blocks, cl);
        //        //    List<Class> newClasses = classes;
        //        //    Console.WriteLine("removing class");
        //        //    newClasses.Remove(cl);
        //        //    Console.WriteLine("removed class");
        //        //    groupingHelper(newBlocks, newClasses);

        //        //}
        //    }
        //    else if (blocks.Count <= this.smallestGroupSizeFound)
        //    {
        //        this.smallestGroupsFound.Add(blocks);
        //    }
        //}


        //protected static async void findSmallgroup(List<Block> blocks, List<Class> classes)
        //{
        //    int currentSmallest;
        //    List<Block> newBlocks = new List<Block>(blocks);
        //    foreach (Class cl in classes)
        //    {
        //        newBlocks = groupClass(newBlocks, cl);
        //    }

        //    currentSmallest = newBlocks.Count;

        //    foreach (Class cl in classes)
        //    {
        //        List<Class> newClasses = new List<Class>(classes); // ?
        //        newClasses.RemoveAt(i);
        //        int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
        //        if (number < currentSmallest)
        //        {
        //            currentSmallest = number;
        //        }
        //    }

        //    Console.WriteLine("Current smallest: {0}", currentSmallest);


        //}

        //// ? make task 
        //protected static async Task<int> findSmallestGroup(List<Block> blocks, List<Class> classes)
        //{
        //    if (classes.Count == 0)
        //    {

        //        return blocks.Count;

        //    }
        //    else
        //    {
        //        List<Block> newBlocks = new List<Block>(blocks);
        //        foreach (Class cl in classes)
        //        {
        //            newBlocks = groupClass(newBlocks, cl);
        //        }

        //        int currentSmallest = newBlocks.Count;

        //        int i = 0;
        //        while (i < classes.Count)
        //        {

        //            List<Class> newClasses = new List<Class>(classes); // ?
        //            newClasses.RemoveAt(i);
        //            int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
        //            if (number < currentSmallest)
        //            {
        //                currentSmallest = number;
        //            }

        //        }
        //        return currentSmallest;
        //    }
        //}

        //protected int numberOfExecutions = 0;
        //protected int findNextSmallestGroup(List<Block> blocks, List<Class> classes, int currentSmallest)
        //{
        //    Console.WriteLine(numberOfExecutions++);
        //    if (blocks.Count > currentSmallest)
        //    {
        //        return currentSmallest;
        //    }
        //    else if (classes.Count == 0) // count must be smaller or equal to smallest thus far
        //    {
        //        return blocks.Count;
        //    }
        //    else
        //    {

        //        List<Block> newBlocks = new List<Block>(blocks);

        //        //Stopwatch stopWatch = new Stopwatch();
        //        //stopWatch.Start();
        //        foreach (Class cl in classes)
        //        {
        //            newBlocks = groupClass(newBlocks, cl);
        //        }

        //        //stopWatch.Stop();
        //        //Console.WriteLine(stopWatch.Elapsed);
        //        if (blocks.Count < currentSmallest)
        //        {
        //            return blocks.Count;
        //        }
        //        //else
        //        //{
        //        Parallel.ForEach(classes, (cl) =>
        //            {
        //                // newBlocks = groupClass(newBlocks, classes[i]);
        //                List<Class> newClasses = new List<Class>(classes); // ?
        //                newClasses = removeClass(newClasses, cl);
        //                int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), cl), newClasses, currentSmallest);
        //                if (number < currentSmallest)
        //                {
        //                    // currentSmallest = number;
        //                    currentSmallest = number;
        //                    return;
        //                }

        //            });
        //            return currentSmallest;
        //        }

                

                

        //        return currentSmallest;

        //        //    foreach(Class cl in classes)
        //        //{
        //        //    // newBlocks = groupClass(newBlocks, classes[i]);
        //        //    List<Class> newClasses = new List<Class>(classes); // ?
        //        //    removeClass(newClasses, cl);
        //        //    int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), cl), newClasses, currentSmallest);
        //        //    if (number < currentSmallest)
        //        //    {
        //        //        // currentSmallest = number;
        //        //        return number;
        //        //    }
        //        //}

        //        //int i = 0;
        //        //while (i < classes.Count)
        //        //{
        //        //    // newBlocks = groupClass(newBlocks, classes[i]);
        //        //    List<Class> newClasses = new List<Class>(classes); // ?
        //        //    newClasses.RemoveAt(i);
        //        //    int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses, currentSmallest);
        //        //    if (number < currentSmallest)
        //        //    {
        //        //        // currentSmallest = number;
        //        //        return number;
        //        //    }

        //        //}


        //    //}
        //}

        


        //// ? make task 
        //protected static int findSmallestGroup(List<Block> blocks, List<Class> classes)
        //{
        //    if(classes.Count == 0)
        //    {
               
        //        return blocks.Count;
         
        //    } 
        //    else
        //    {
        //        List<Block> newBlocks = new List<Block>(blocks);
        //        foreach(Class cl in classes)
        //        {
        //            newBlocks = groupClass(newBlocks, cl);
        //        }

        //        int currentSmallest = newBlocks.Count;

        //        int i = 0;
        //        while (i < classes.Count)
        //        {

        //            List<Class> newClasses = new List<Class>(classes); // ?
        //            newClasses.RemoveAt(i);
        //            int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
        //            if(number < currentSmallest)
        //            {
        //                currentSmallest = number;
        //            }

        //        }
        //        return currentSmallest;
        //    }
        //}


        //protected List<Block> checkAllPossibleGroups(List<Block> blocks, List<Class> classes)
        //{

        //}


        //protected void helpMe(List<Block> blocks, List<Class> classes)
        //{
        //    if(classes.Count == 0)
        //    {
        //        if(blocks.Count < this.smallestGroupSizeFound)
        //        {
        //            this.smallestGroupSizeFound = blocks.Count;
        //        }
        //    }
        //    else
        //    {
        //        //List <Block> newBlocks = new List<Block>(blocks);
        //        //foreach(Class cl in classes)
        //        //{
        //        //    groupClass(newBlocks, cl);
        //        //}
        //        //if(newBlocks.Count < this.smallestGroupSizeFound)
        //        //{
        //        //    this.smallestGroupSizeFound = newBlocks.Count;
        //        //}
        //        //List<Block> newBlocks = blocks;

        //        int i = 0;
        //        while (i < classes.Count)
        //        {
        //            List<Class> what = new List<Class>(classes); // ?
        //            what.RemoveAt(i);
        //            helpMe(groupClass((new List<Block>(blocks)),classes[i]), what);
        //        }
        //    }
        //}









































    } // end class Schedule
}
