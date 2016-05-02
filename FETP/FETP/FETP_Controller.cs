using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Globalization;


namespace FETP
{
    /// <summary>
    /// Placeholder
    /// </summary>
    //Author: Victor Rogers, Benjamin Etheredge
    //Date: 3-25-2016
    //Modifications:  
    //Date(s) Tested:
    //Approved By:
    public static class FETP_Controller
    {

        #region Methods
        /// <summary>
        /// Computes variance of all blocks 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns></returns>
        public static int ComputeVarianceOfBlocks(List<Block> blocks)
        {
            int newAverage = 0;
            foreach (Block block in blocks)
            {
                newAverage += block.Enrollment;
            }
            newAverage /= blocks.Count;

            int newVariance = 0;
            foreach (Block block in blocks)
            {
                int difference = block.Enrollment - newAverage;
                newVariance += (difference * difference);
            }

            return newVariance / blocks.Count;
        }


        /// <summary>
        /// Determines if the two input classes overlap.
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool doClassesOverlap(Class class1, Class class2)
        {
            return (doClassDaysOverlap(class1, class2) && doClassTimesOverlap(class1, class2)); // Broke up to aid readability
        }


        /// <summary>
        /// Determines if the two input classes share any days in common
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool doClassDaysOverlap(Class class1, Class class2)
        {
            return (getNumberOfDaysInCommon(class1, class2) > 0);
        }


        /// <summary>
        /// Determines if the two input classes have times that overlap
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool doClassTimesOverlap(Class class1, Class class2)
        {
            return (
              (class1.StartTime >= class2.StartTime && class1.StartTime <= class2.EndTime) // does class1 start during class2
              || (class1.EndTime >= class2.StartTime && class1.EndTime <= class2.EndTime) // or does class1 end during class2 
              || (class2.StartTime >= class1.StartTime && class2.StartTime <= class1.EndTime) // or does class2 start during class1
              || (class2.EndTime >= class1.StartTime && class2.EndTime <= class1.EndTime) // or does class2 end during class1
            ); 
        }


        /// <summary>
        /// Gets the number of overlapping days between two input classes
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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


        /// <summary>
        /// Determines the number of classes in the list of classes that the inClass
        /// overlaps with.
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="inClass"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static int getNumberOfOverlappingClasses(List<Class> classes, Class inClass)
        {
            int overlappingClasses = 0; 
            foreach (Class cl in classes)
            {
                if (cl != inClass && doClassesOverlap(cl, inClass)) overlappingClasses++;
            }
            return overlappingClasses;
        }


        /// <summary>
        /// Checks if there are any overlapping classes in the list of classes.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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

        /// <summary>
        /// Takes in a list of class groups and a class. The method then returns a
        /// new list of class groups with the class inserted into the first possible
        /// group.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns>List of blocks which are grouped classes</returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static List<Block> GroupClasses(List<Class> classes)
        {
            List<Block> groupedClasses = new List<Block>();

            foreach (Class cl in classes)
            {
                // first, build a list of all indexes of blocks where the class could be placed
                List<int> indexes = new List<int>();
                int i = 0;
                while (i < groupedClasses.Count) //TODO: convert to for loop
                {
                    if (groupedClasses[i].doesClassOverlapWithBlock(cl))
                    {
                        indexes.Add(i);
                    }
                    i++;
                }

                // if no overlapping blocks where found, create a new one
                if (indexes.Count == 0)
                {
                    groupedClasses.Add(new Block(cl));
                }
                // else find which block to insert class into
                else //TODO: improve determination of which block to insert class into
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


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="inClass"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
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

        #region Data Constraints Validators

        /// <summary>
        /// Validates number of days
        /// </summary>
        /// <param name="numberOfDays">Number of days available for exam scheduling</param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool ValidateNumberOfDays(string numberOfDays)
        {
            bool isValid = false;
            int value;
            if((Int32.TryParse(numberOfDays, out value)) && (value >= Schedule.MIN_NUMBER_OF_DAYS_FOR_EXAMS) && (value <= Schedule.MAX_NUMBER_OF_DAYS_FOR_EXAMS))
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examsStartTime">Time for exams to start at</param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool ValidateExamsStartTime(string examsStartTime)
        {
            bool isValid = false;

            //TODO remove
            ////string examsStartTime parameter is in "hh:mm" format, not minutes.
            //DateTime tempStartTime = Convert.ToDateTime(examsStartTime);
            //TimeSpan startTime = TimeSpan.ParseExact(tempStartTime.TimeOfDay.Ticks);
            try //TODO: need better error catching for invalid times
            {
                TimeSpan startTime = TimeSpan.ParseExact(examsStartTime, @Schedule.TIME_FORMAT_FROM_GUI, CultureInfo.InvariantCulture);
                // Convert Constants to more usable format
                TimeSpan minStartTime = TimeSpan.ParseExact(Schedule.MIN_START_TIME, @"hhmm", CultureInfo.InvariantCulture);
                TimeSpan maxStartTime = TimeSpan.ParseExact(Schedule.MAX_START_TIME, @"hhmm", CultureInfo.InvariantCulture);

                if ((startTime >= minStartTime) && (startTime <= maxStartTime))
                {
                    isValid = true;
                }
            }
            catch
            {

            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examsLength">Length exams will run</param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool ValidateExamsLength(string examsLength)
        {
            bool isValid = false;

            int length;
            if (Int32.TryParse(examsLength, out length) && (length >= Schedule.MIN_EXAM_LENGTH_IN_MINUTES) && (length <= Schedule.MAX_EXAM_LENGTH_IN_MINUTES))
            {
                isValid = true;   
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeBetweenExams">Length of the break time between exams</param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool ValidateTimeBetweenExams(string timeBetweenExams)
        {
            bool isValid = false;

            int length;
            if (Int32.TryParse(timeBetweenExams, out length) && (length >= Schedule.MIN_BREAK_TIME_IN_MINUTES) && (length <= Schedule.MAX_BREAK_TIME_IN_MINUTES))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lunchLength">Length of lunch time</param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 4-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool ValidateLunchLength(string lunchLength)
        {
            bool isValid = false;

            int length;
            if (Int32.TryParse(lunchLength, out length) && (length >= Schedule.MIN_LUNCH_LENGTH_IN_MINUTES) && (length <= Schedule.MAX_LUNCH_LENGTH_IN_MINUTES))
            {
                isValid = true;
            }

            return isValid;
        }
            #endregion


        #endregion


        #region Overloaded Operators
        //Overloaded operators here

        #endregion
    }
}
