using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;  // allows times to be different pased on local ? may can be removed

namespace FETP
{
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
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
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
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
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


        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        public static int getNumberOfTimeSlotsAvailable(Schedule schedule)
        {
            return getNumberOfTimeSlotsAvailablePerDay(schedule) * schedule.NumberOfDays;
        }


        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
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

        // Checks if classes have overlapping times
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

        // takes in a list of class groups and a class
        // returns a new list of class groups with class inserted into first possible group
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        public static List<Block> groupClass(List<Block> blocks, Class inClass)
        {
            bool isInserted = false;
            int i = 0;

            while (i < blocks.Count && !(isInserted = blocks[i++].addClass(inClass))) ; // ? i <3 my while loops that terminate in a semicolen

            if (!isInserted)
                blocks.Add(new Block(inClass));
            return blocks;
        }


        // basic grouping for testing  
        /**************************************************************************\
        Method:  
        Description: 
        \**************************************************************************/
        public static List<Block> coalesceClassesTogether(List<Class> classes)
        {
            List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

            foreach (Class cl in classes)
            {
                classesToBeGrouped = groupClass(classesToBeGrouped, cl); // ? clean this up
            }
            return classesToBeGrouped;
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

        
    }
}
