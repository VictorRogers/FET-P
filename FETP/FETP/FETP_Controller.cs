using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Globalization;


namespace FETP
{
    // ?? 
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
        /**************************************************************************\
        Schedule - Constant Data Members 
        \**************************************************************************/
        //private const string CLASS_LENGTH_TO_START_IGNORING = "0245"; 
        //private const string HOUR_TO_BEGIN_IGNORE_CLASS = "1800";

        //// ? this might need to be moved
        //// Programmer: Ben
        //// takes in an open data file and returns a list of all the classes
        ///**************************************************************************\
        //Method: readInputDataFile
        //Description: Reads in data file and constructs list of all classes
        //             in the file. Does not add classes in that fall into the
        //             criteria of ignorable classes
        //\**************************************************************************/
        //public static List<Class> readInputDataFile(string inFileName)
        //{

        //    // Make boundaries of ignored classes more usable
        //    TimeSpan ignoreClassLength = TimeSpan.ParseExact(CLASS_LENGTH_TO_START_IGNORING, @"hhmm", CultureInfo.InvariantCulture);
        //    TimeSpan ignoreClassStartTime = TimeSpan.ParseExact(HOUR_TO_BEGIN_IGNORE_CLASS, @"hhmm", CultureInfo.InvariantCulture);

        //    List<Class> allClasses = new List<Class>(); // list of all classes to be returned

        //    FileStream inFile = File.OpenRead(@inFileName);
        //    var reader = new StreamReader(inFile);

        //    reader.ReadLine(); // skip description line

        //    while (!reader.EndOfStream)
        //    {
        //        // ? possibly change var to string
        //        var line = reader.ReadLine(); // reads in next line
        //        var values = line.Split(','); // splits into days/times and enrollement
        //        var daysAndTimes = values[0].Split(' '); // chops up the days and times to manageable sections

        //        TimeSpan startTime = TimeSpan.ParseExact(daysAndTimes[1], @"hhmm", CultureInfo.InvariantCulture); // 1 postion is the start time, changes formated time to bw more usable 
        //        TimeSpan endTime = TimeSpan.ParseExact(daysAndTimes[3], @"hhmm", CultureInfo.InvariantCulture); // 3 position is the end time, changes formated time to bw more usable 

        //        // Checks if class should not be ignored before continuing execution
        //        if ((startTime < ignoreClassStartTime) && (endTime - startTime < ignoreClassLength))
        //        {
        //            List<DayOfWeek> days = new List<DayOfWeek>(); // days the class meets
        //            foreach (char day in daysAndTimes[0].ToCharArray()) // changes days from string of chars to list of DayOfWeek type
        //            {
        //                switch (day)
        //                {
        //                    case 'M':
        //                        days.Add(DayOfWeek.Monday);
        //                        break;
        //                    case 'T':
        //                        days.Add(DayOfWeek.Tuesday);
        //                        break;
        //                    case 'W':
        //                        days.Add(DayOfWeek.Wednesday);
        //                        break;
        //                    case 'R':
        //                        days.Add(DayOfWeek.Thursday);
        //                        break;
        //                    case 'F':
        //                        days.Add(DayOfWeek.Friday);
        //                        break;
        //                }
        //            }

        //            int enrollment = Int32.Parse(values[1]); // enrollement values should be in 1 position

        //            allClasses.Add(new Class(startTime, endTime, enrollment, days)); // add new Class to list

        //        }
        //    }
        //    Schedule.AllClasses = allClasses;

        //    // ? rewrite
        //    return allClasses;

        //} 

        //// ? this might need to be moved
        ///**************************************************************************\
        //Method: readInputConstraintsFile
        //Description: Reads in constraints file and intializes static schedule
        //             data members
        //\**************************************************************************/
        //public static void readInputConstraintsFile(string inFileName)
        //{
        //    FileStream inFile = File.OpenRead(@inFileName);
        //    var reader = new StreamReader(inFile); // ?

        //    Schedule.NumberOfDays = Int32.Parse(reader.ReadLine());
        //    Schedule.ExamsStartTime = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
        //    Schedule.ExamsLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
        //    Schedule.TimeBetweenExams = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
        //    Schedule.LunchLength = TimeSpan.ParseExact(reader.ReadLine(), @"hhmm", CultureInfo.InvariantCulture);
        //}


        //Used for random number generation
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();


        /**************************************************************************\
        Method: RandomFloatBetween01 
        Description: Will return a random float between 0 and 1 
        \**************************************************************************/       
        public static float RandomFloatBetween01()
        {
            lock (syncLock)
            {
                return (float)random.NextDouble();
            }
        }
        

        // Checks if two classes overlap
        /**************************************************************************\
        Method: doClassesOverlap
        Description: Determines if the two input classes overlap.
        \**************************************************************************/
        public static bool doClassesOverlap(Class class1, Class class2)
        {
            return (doClassDaysOverlap(class1, class2) && doClassTimesOverlap(class1, class2)); // Broke up to aid readability
        }


        /**************************************************************************\
        Method: doClassDaysOverlap
        Description: Determines if the two input classes share any days in common
        \**************************************************************************/
        public static bool doClassDaysOverlap(Class class1, Class class2)
        {
            return (getNumberOfDaysInCommon(class1, class2) > 0);
        }

        
        /**************************************************************************\
        Method: doClassTimesOverlap
        Description: Determines if the two input classes have times that overlap
        \**************************************************************************/
        public static bool doClassTimesOverlap(Class class1, Class class2)
        {
            return (
              (class1.StartTime >= class2.StartTime && class1.StartTime <= class2.EndTime) // does class1 start during class2
              || (class1.EndTime >= class2.StartTime && class1.EndTime <= class2.EndTime) // or does class1 end during class2 
              || (class2.StartTime >= class1.StartTime && class2.StartTime <= class1.EndTime) // or does class2 start during class1
              || (class2.EndTime >= class1.StartTime && class2.EndTime <= class1.EndTime) // or does class2 end during class1
            ); 
        }


        /**************************************************************************\
        Method: getNumberOfDaysInCommon
        Description: Gets the number of overlapping days between two input classes
        \**************************************************************************/
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


        /**************************************************************************\
        Method: getNumberOfOverlappingClasses
        Description: Determines the number of classes in the list of classes 
                     that the inClass overlaps with
        \**************************************************************************/
        public static int getNumberOfOverlappingClasses(List<Class> classes, Class inClass)
        {
            int overlappingClasses = 0; 
            foreach (Class cl in classes)
            {
                if (cl != inClass && doClassesOverlap(cl, inClass)) overlappingClasses++;
            }
            return overlappingClasses;
        }


        /**************************************************************************\
        Method: doAnyClassesOverlap
        Description: Checks if there are any overlapping classes in the list of
                     classes
        \**************************************************************************/
        public static bool doAnyClassesNotOverlap(List<Class> classes)
        {
            foreach (Class class1 in classes)
            {
                foreach (Class class2 in classes)
                {
                    if (!doClassesOverlap(class1, class2))
                        return true;
                }
            }
            return false;
        }



        /**************************************************************************\
        Schedule - Constant Data Members 
        \**************************************************************************/
        // takes in a list of class groups and a class
        // returns a new list of class groups with class inserted into first possible group
        /**************************************************************************\
        method:  
        description: 
        \**************************************************************************/
        public static List<Block> GroupClass(List<Block> blocks, Class inclass)
        {
            bool isinserted = false;
            int i = 0;
            while(i<blocks.Count && !isinserted)
            {
                if(blocks[i].doesClassOverlapWithBlock(inclass))
                {
                    blocks[i].addClass(inclass);
                    isinserted = true;
                    i++;
                }
            }
            if (!isinserted)
            {
                blocks.Add(new Block(inclass));
            }
            return blocks;
        }


        // basic grouping for testing  
        /**************************************************************************\
        method:  
        description: 
        \**************************************************************************/
        public static List<Block> CoalesceClassesTogether(List<Class> classes)
        {
            List<Block> classestobegrouped = new List<Block>(); // variable to contain the list of all grouped classes

            foreach (Class cl in classes)
            {
                classestobegrouped = GroupClass(classestobegrouped, cl); // ? clean this up
            }
            return classestobegrouped;
        }

        // ?????
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
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

        public static List<Block> GroupClasses(List<Class> classes)
        {
            List<Block> groupedClasses = new List<Block>();
            //foreach (Class cl in classes)
            //{
            //    bool doesItOverlap = false;
            //    foreach (Block block in groupedClasses)
            //    {
            //        if (block.doesClassOverlapWithBlock(cl)) // ? changed to 0. makes no difference when making smallest classes, but should make it work with blank days
            //        {
            //            block.addClass(cl);
            //            doesItOverlap = true;
            //            break; // ?bug THIS WASN'T THERE :'( // maybe not a bug due to ordering?
            //        }
            //    }
            //    if (!doesItOverlap)
            //    {
            //        groupedClasses.Add(new Block(cl));
            //    }
            //}
            //return groupedClasses;



            foreach (Class cl in classes)
            {
                List<int> indexes = new List<int>();
                int i = 0;
                while (i < groupedClasses.Count)
                {
                    if (groupedClasses[i].doesClassOverlapWithBlock(cl))
                    {
                        indexes.Add(i);
                    }
                    i++;
                }

                if (indexes.Count == 0)
                {
                    groupedClasses.Add(new Block(cl));
                }
                else
                {
                    int indexOfLargest = 0;
                    int currentLargest = 0;
                    foreach (int index in indexes)
                    {
                        if (groupedClasses[index].ClassesInBlock.Count > currentLargest)
                        {
                            currentLargest = groupedClasses[index].ClassesInBlock.Count;
                            indexOfLargest = index;
                        }
                    }
                    //indexOfLargest = indexes[ GA_Controller.GetRandomInt() % indexes.Count];
                    groupedClasses[indexOfLargest].addClass(cl);
                }
            }

            return groupedClasses;

        }
     

        public static int ComputeVarianceOfBlocks(List<Block> blocks)
        {
            int newaverage = 0;
            foreach (Block block in blocks)
            {
                newaverage += block.Enrollment;
            }
            newaverage /= blocks.Count;

            int newvariance = 0;
            foreach (Block block in blocks)
            {
                int difference = block.Enrollment - newaverage;
                newvariance += (difference * difference);
            }

            return newvariance / blocks.Count;


        }
    }

    
}
