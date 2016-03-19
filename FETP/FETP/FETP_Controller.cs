using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;  // allows times to be different pased on local ? may can be removed

/*

    Ben Notes:
        swaped enrollment and days inorder for the call to the class constructor from block cleaner

        do we really want to make block a class?
            it gets weird with construction
            its not REALLY a class
            it is probably for the best

        
            could change and give it property isMWF to determine days
            days of blocks SHOULD only be MWF or TR

        timeslot class
            doesn't seem to be needed
            only thing to think to do is limit number of slots, but thats not needed


        remember .orderby().thenby()
        can now sort by enrollment or overlapping days
        can sort by any combination of attributes

        do not write validateDataFiles. we assume they are correct
        can write those later for extra

        fitness function
            one weight can be difference of hours between exam time and class time multiplied by number of people affected
            second weight can be variance in days.
            less wieght for after lunch break
            weight for how many students are consecutive?
            weight for internal and external variance of student enrollments in groups and days5

        ineffeciencies in loaders. we could make them part of the classes.
            its calling the class constructor to make a new class then creating a new one with it?
            but its fine for now

        look into asychronos capabilites of c#

        look into multiprocessing stuff for when running GE

        still need a way to determin if it is possible to schedule all classes together

        new fringe case, do we want to give them the ability to schedule overlapping exams if we can't coalesce small enough?
            we can say how many students will be effected and let them decide that

        ask professor about best place to put some of these functions (in class or in controller)
            which is the better practice

        change name of lengthOfExamDay to available time

        change all TimeSlots to Timeslot

        group with less populated classes to help spread out people
        unless all are coalesced into MTF and TR
        need option to not do that if there's enough space?
            probably not. if the classes couldn't be taken at the same time, then we should show them they don't need all those time slots?
        how do we find the smallest possible grouping?
            possible different groupings result in differnt number of groups?

        when outputing a block, output the info of the most common time

        not sure if i like schedule class we have now. rework?

        will sorting thenby enrollment make better results?

        possibly don't let block inherit from class but from a new class callled course information

        could really break up and stop passing around classes. could just pass around parameters

        can we not just break up the largest groups?
            break off smaller courses from it to fill slots
        could break off based on how far the internal classes are from their own time times number enrolled?
            keep in mind we're trying to keep groups large and avoid spreading people out maybe?

        you have to pick on class and base mutual exclussion of that one
            class A could be mutually exclussive with class B.
            Class B could be mutually exclussive with Class C
            class A is not necissarily mutually exclusive with Class C
            ????????????????

        Victor, if you're reading this, you were right. I should have went with your idea of schedule just having the list of all the gorups
        My method might have been useful if we also cared about when exam days were

        this is getting so ugly it hurts :'(
        I'm so close to something though...

        Can redo ordering with List's built in functions. can rewrite checking if item is in list as well




*/



namespace FETP
{
    

    

    

    //// playing with TimeSlot class
    ////hmmmm
    //// ? did i really just make this?
    //// it's LITERALLY just a block... damn
    //public class Timeslot : Class
    //{
    //    protected Block groupOfClasses;

    //    public Timeslot(TimeSpan inStartTime, TimeSpan inEndTime, Block inBlock = null, List<DayOfWeek> inDaysMeet = null, int inEnrollment = 0)
    //        : base(inStartTime, inEndTime, inDaysMeet, inEnrollment)
    //    {
    //        this.groupOfClasses = inBlock;
    //    }



    //    public override void Display()
    //    {
    //        base.Display();
    //    }

    //}

        // ? oh gawd this hurts. why ben? why? why did you do this? it's not good
    //public class Day
    //{
    //    public List<Block> blocks; // ? lazy
    //    protected TimeSpan dayStartTime;
    //    protected TimeSpan lunchStartTime;
    //    protected TimeSpan lunchEndTime;
        

    //    public Day(TimeSpan dayStartTime, TimeSpan lunchStartTime = TimeSpan.Zero, TimeSpan lunchEndTime = TimeSpan.Zero, List<Block> inBlocks = null) // 0 because there won't always be lunch
    //    {
    //        this.dayStartTime = dayStartTime;
    //        this.lunchStartTime = lunchStartTime;
    //        this.lunchEndTime = lunchEndTime;
    //        this.blocks = inBlocks;
    //    }



    //    public void Display()
    //    {
    //        foreach (Timeslot block in blocks)
    //            block.Display();
    //    }
    //}

   



    

    /**************************************************************************\
    Class: FETP_Controller (Final Exam Timetabling Problem Controller)
    Description: This class contains all of the primary functions used for 
    reading in data, adjusting data, outputting data, and running starting the 
    process of running the genetic algorithm. This is the primary interface 
    between the front-end and the back-end. It reads from the data file, formats 
    the data for use by the GA_Controller, receives the solutions from the 
    GA_Controller, and sends them back to the front-end for display in the GUI.
    \**************************************************************************/
    public static class FETP_Controller
    {
        // These are the values that determine the boundaries of classes to be ignored
        private const string CLASS_LENGTH_TO_START_IGNORING = "0245";
        private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";
        private const string TIME_EXAMS_MUST_END_BY = "1700";

        // Programmer: Ben
        // takes in an open data file and returns a list of all the classes
        public static List<Class> readInputDataFile(string inFileName)
        {

            // Make boundaries of ignored classes more usable
            TimeSpan ignoreClassLength = TimeSpan.ParseExact(CLASS_LENGTH_TO_START_IGNORING, @"hhmm", CultureInfo.InvariantCulture);
            TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(HOUR_TO_BEGIN_IGNORE_CLASS, @"hhmm", CultureInfo.InvariantCulture);

            List<Class> allClasses = new List<Class>(); // list of all classes to be returned

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

                    allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list

                }
            }

            return allClasses;

        } // end readInputDataFile

        // ? don't know what to do with this info yet
        public static Schedule readInputConstraintsFile(string inFileName)
        {
            FileStream inFile = File.OpenRead(@inFileName);
            var reader = new StreamReader(inFile);

            int numberOfDays = Int32.Parse(reader.ReadLine());
            TimeSpan startTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            TimeSpan examLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            TimeSpan timeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
            TimeSpan lunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);

            return new Schedule(numberOfDays, startTime, examLength, timeBetweenExams, lunchLength);

        }

        public static int getNumberOfTimeSlotsAvailable(Schedule schedule)
        {
            return getNumberOfTimeSlotsAvailablePerDay(schedule) * schedule.NumberOfDays;
        }

        public static int getNumberOfTimeSlotsAvailablePerDay(Schedule schedule)
        {
            TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go

            TimeSpan lengthOfExamDay = latestTime - schedule.ExamsStartTime; // Figure out how much time available for exams

            // if the lunch time is longer than the break time, account for it and the extra break time it will give you
            if (schedule.LunchLength > schedule.TimeBetweenExams)
            {
                lengthOfExamDay -= (schedule.LunchLength - schedule.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
            }


            TimeSpan examFootprint = schedule.ExamsLength + schedule.TimeBetweenExams;

            int numberOfExams = 0;
            // ? bug if exam break is too big
            while ((lengthOfExamDay - schedule.ExamsLength) >= TimeSpan.Zero)
            {
                lengthOfExamDay -= schedule.ExamsLength;
                numberOfExams++;

                // checks if a break is needed due to their being room for another exam after a break
                if ((lengthOfExamDay - (schedule.TimeBetweenExams + schedule.ExamsLength) >= TimeSpan.Zero))
                {
                    lengthOfExamDay -= schedule.TimeBetweenExams;
                }
            }
            return numberOfExams;
        }

        // ? original
        //// takes in a list of classes and coalesces them into a list of blocks of classes
        //public static List<Block> coalesceClassesTogether(List<Class> classes)
        //{
        //    List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes
            
        //    foreach (Class cl in classes)
        //    {
        //        bool doesItOverlap = false;
        //        foreach(Block block in classesToBeGrouped)
        //        {
        //            if (doClassesOverlap(cl, block.ClassesInBlock[0])) // ? changed to 0. makes no difference when making smallest classes, but should make it work with blank days
        //            {
        //                block.addClass(cl);
        //                doesItOverlap = true;
        //                break; // ?bug THIS WASN'T THERE :'( // maybe not a bug due to ordering?
        //            }
        //        }
        //        if(!doesItOverlap)
        //            classesToBeGrouped.Add(new Block(cl));
        //    }
        //    return classesToBeGrouped;

        //}

        // takes in a list of classes and coalesces them into a list of blocks of classes
        



        //// takes in a list of classes and coalesces them into a list of blocks of classes
        //public static List<Block> coalesceClassesTogether(List<Class> classes, List<Block> groups)
        //{
        //    // ? check if possible maybe?
        //    //

        //    if (groups == null)
        //    {
        //        groups = new List<Block>(); // Variable to contain the list of all grouped classes
        //    }

        //    foreach (Class cl in classes)
        //    {
        //        bool isFound = false;

        //        TimeSpan weight = TimeSpan.Zero;
        //        while (!isFound) // ?hello infinity
        //        {
                    
        //            foreach (Block block in groups)
        //            {
        //                if (block.StartTime - cl.StartTime < weight && (block.ClassesInBlock == null || doClassesOverlap(cl, block.ClassesInBlock[0]))) // could reference null, but should drop out
        //                {
        //                    block.addClass(cl);
        //                    isFound = true;
        //                    break; // ? BAD BAD BAD :'(
        //                }
        //            }
        //            weight += TimeSpan.FromMinutes(30);
        //        }
                
        //    }
        //    return groups;
        //}

        //public static getClosest

        //// ? need to rewrite and combine these
        //// takes in a list of classes and coalesces them into a list of blocks of classes
        //public static List<Block> coalesceClassesTogether(List<Class> classes)
        //{
        //    List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

        //    // ? oh gawd this hurts
        //    List<Block> allGroups = new List<Block>;
            
        //    foreach (Class cl in classes)
        //    {
        //        bool doesItOverlap = false;
        //        foreach (Block block in classesToBeGrouped)
        //        {
        //            if (doClassesOverlap(cl, block.ClassesInBlock[0])) // ? changed to 0. makes no difference when making smallest classes, but should make it work with blank days
        //            {
        //                block.addClass(cl);
        //                doesItOverlap = true;
        //            }
        //        }
        //        if (!doesItOverlap)
        //            classesToBeGrouped.Add(new Block(cl));
        //    }
        //    return classesToBeGrouped;

        //}


        //public static List<Day> coalesceClassesIntoDays(Schedule schedule, List<Class> classes)
        //{
        //    //List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

        //    //for(int i = 0; i < schedule.NumberOfDays; i++)
        //    //{
        //    //    Day newDay = new Day();
        //    //    TimeSpan examCurrentTime = schedule.ExamsStartTime;
        //    //    for(int j = 0; j < getNumberOfTimeSlotsAvailablePerDay(schedule); j++)
        //    //    {
        //    //        Block newBlock
        //    //    }
        //    //}

        //    //return classesToBeGrouped;

        //     List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

        //     foreach (Class cl in classes)
        //     {
        //         bool doesItOverlap = false;
        //         foreach (Day day in schedule.days)
        //         {
        //             foreach (day )
        //             if (doClassesOverlap(cl, day.block))
        //             {
        //                 block.addClass(cl);
        //                 doesItOverlap = true;
        //             }
        //         }
        //         if (!doesItOverlap)
        //             classesToBeGrouped.Add(new Block(cl));
        //     }
        //     return classesToBeGrouped;


        // }



        //// fills in days with blank timeslots
        //public static List<Block> createShellDays(Schedule schedule)
        //{
        //    // List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

        //    List<Block> groups = new List<Block>();

        //    for (int i = 0; i < schedule.NumberOfDays; i++)
        //    {
        //        TimeSpan examCurrentTime = schedule.ExamsStartTime;
        //        for (int j = 0; j < FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(schedule); j++)
        //        {
        //            groups.Add(new Block(examCurrentTime, examCurrentTime + schedule.ExamsLength));
        //            // ? check if lunch and account for it here
        //            examCurrentTime += schedule.ExamsLength + schedule.TimeBetweenExams;
        //        }
        //    }

        //    return groups;
        //}
        /*
        // fills in days with blank timeslots
        public List<Days> createShellDays(int numberOfDays, int numberOfTimeslotsPerDay, int examsStartTime, TimeSpan examsLength, TimeSpan timeBetweenExams)
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
       

        /*
        // ? switching up style of functions here to make coupling ?lower?
        public static List<Days> makeBlankDays(TimeSpan startTime, TimeSpan endTime, TimeSpan examLength)
        */

        // Checks if two classes overlap
        public static bool doClassesOverlap(Class class1, Class class2)
        {
            return (doClassDaysOverlap(class1, class2) && doClassTimesOverlap(class1, class2)); // Broke up to aid readability
        }

        public static bool doClassDaysOverlap(Class class1, Class class2)
        {
            return (getNumberOfDaysInCommon(class1, class2) > 0);
        }

        // Checks if classes have overlapping times
        public static bool doClassTimesOverlap(Class class1, Class class2)
        {
            return (
              (class1.StartTime >= class2.StartTime && class1.StartTime <= class2.EndTime) // does class1 start during class2
              || (class1.EndTime >= class2.StartTime && class1.EndTime <= class2.EndTime) // or does class1 end during class2 
              || (class2.StartTime >= class1.StartTime && class2.StartTime <= class1.EndTime) // or does class2 start during class1
              || (class2.EndTime >= class1.StartTime && class2.EndTime <= class1.EndTime) // or does class2 end during class1
            ); 
        }

        //// checks if class overlaps with any class in a block
        //public static bool doesClassOverlapBlock(Block block, Class inClass)
        //{
        //    foreach (Class cl in block.ClassesInBlock)
        //    {
        //        if (!doClassesOverlap(cl, inClass))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        // Helper function to find the total days two classes have in common.

        public static int getNumberOfDaysInCommon(Class class1, Class class2)
        {
            int daysInCommon = 0;
            foreach(DayOfWeek dayFromClass1 in class1.DaysMeet)
            {
                foreach(DayOfWeek dayFromClass2 in class2.DaysMeet)
                {
                    if (dayFromClass1 == dayFromClass2) daysInCommon++;
                }
            }
            return daysInCommon;
        }

        public static int getNumberOfOverlappingDays(List<Class> classes, Class inClass)
        {
            int overlappingClasses = 0; 
            foreach (Class cl in classes)
            {
                if (cl != inClass && FETP_Controller.doClassesOverlap(cl, inClass)) overlappingClasses++;
            }
            return overlappingClasses;
        }

        public static bool doAnyClassesOverlap(List<Class> classes)
        {
            foreach (Class class1 in classes)
            {
                foreach (Class class2 in classes)
                {
                    if (doClassesOverlap(class1, class2))
                        return true;
                }
            }
            return false;
        }

        //// ? maybe correct?
        //public static int getSmallestPossibleGrouping(List<Class> classes)
        //{
        //    return (coalesceClassesTogether(sortClassesByOverlappingDays(classes))).Count;
        //}

        // Find if any classes overlap in list of classes


        //// Checks if enough timeslots are available
        //public static bool areThereEnoughTimeslots(Schedule schedule, List<Class> classes)
        //{
        //    return (getNumberOfTimeSlotsAvailable(schedule) >= getSmallestPossibleGrouping(classes));
        //}

        /*
        // ? in theory, this will automatically find what days a block meets.
        // a block should only meet on MWF or TR
        //return a key value
        //      Day: #
        //      Number of those days: #
        public static List<DayOfWeek> getMostCommonDays(List<Class> classes)
        {
            foreach (Class class1 in classes)
            {
                foreach (Class class2 in classes)
                {
                    if (doClassDaysOverlap(class1, class2))
                        return true;
                }
            }
            return false;
        }
        */

        // Prototype function for example of how to sort
        public static List<Class> sortClassesByEnrollment(List<Class> classes)
        {
            return classes.OrderByDescending(c => c.Enrollment).ToList();
        }

        // Prototype function for example of how to sort
        public static List<Class> sortClassesByOverlappingDays(List<Class> classes)
        {
            return classes.OrderByDescending(c => getNumberOfOverlappingDays(classes, c)).ToList();
        }

        //public static TimeSpan putInClosestPossibleBlock()
        //{
        //    // first look for closest block with start time, then go to next largest

        //        // preference to being later then earlier


        //}

    }
}
