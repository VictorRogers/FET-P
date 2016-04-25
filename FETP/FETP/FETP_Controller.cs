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
    public static class FETP_Controller
    {
        #region Utilities
        //Used for random number generation
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        /// <summary>
        /// Returns a random float between 0 and 1
        /// </summary>
        /// <returns>A random float between 0 and 1</returns>
        public static float RandomFloatBetween01()
        {
            lock (syncLock)
            {
                return (float)random.NextDouble();
            }
        }
        

        /// <summary>
        /// Returns a random integer
        /// </summary>
        /// <returns>A random integer</returns>
        public static int GetRandomInt()
        {
            int upperRange = Int32.MaxValue;
            int lowerRange = 0;

            lock (syncLock)
            {
                return random.Next(lowerRange, upperRange);
            }
        }

        #endregion


        #region Data Constants
        /// <summary>
        /// Placeholder
        /// </summary>
        public const int THREAD_LIMIT = 6;

        #endregion


        #region Data Members
        //Add data members here

        #endregion


        #region Properties
        //Properties here

        #endregion


        #region Methods
        /// <summary>
        /// Placeholder 
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
        /// <param name="blocks"></param>
        /// <param name="inclass"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Takes in a list of class groups and a class. The method then returns a
        /// new list of class groups with the class inserted into the first possible
        /// group.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static List<Block> GroupClasses(List<Class> classes)
        {
            List<Block> groupedClasses = new List<Block>();

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


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static List<Block> CoalesceClassesTogether(List<Class> classes)
        {
            List<Block> classestobegrouped = new List<Block>(); // variable to contain the list of all grouped classes

            foreach (Class cl in classes)
            {
                classestobegrouped = GroupClass(classestobegrouped, cl); // TODO: clean this up // i don't know what this means anymore
            }
            return classestobegrouped;
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="inClass"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfDays">Number of days available for exam scheduling</param>
        /// <returns></returns>
        public static bool ValidateNumberOfDays(String numberOfDays)
        {
            bool isValid = false;
            int value;
            if(Int32.TryParse(numberOfDays, out value) && value > Schedule.MIN_NUMBER_OF_DAYS_FOR_EXAMS && value < Schedule.MAX_NUMBER_OF_DAYS_FOR_EXAMS)
            {

            }
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examsStartTime">Time for exams to start at</param>
        /// <returns></returns>
        public static bool ValidateExamsStartTime(String examsStartTime)
        {
            bool isValid = false;
            //TODO: implement
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examsLength">Length exams will run</param>
        /// <returns></returns>
        public static bool ValidateExamsLength(String examsLength)
        {
            bool isValid = false;
            //TODO: implement
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeBetweenExams">Length of the break time between exams</param>
        /// <returns></returns>
        public static bool ValidateTimeBetweenExams(String timeBetweenExams)
        {
            bool isValid = false;
            //TODO: implement
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lunchLength">Length of lunch time</param>
        /// <returns></returns>
        public static bool ValidateLunchLength(String lunchLength)
        {
            bool isValid = false;
            //TODO: implement
            return isValid;
        }


        ///// <summary>
        ///// Placeholder
        ///// </summary>
        ///// <param name="dataFileLocation"></param>
        ///// <param name="constraintsFileLocation"></param>
        //public static void Run(string dataFileLocation, string constraintsFileLocation)
        //{
        //    Schedule.readInputConstraintsFile("../../../../Example Data/Ben Made Constraints Sample.txt");
        //    Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv"); // ? throw exceptions for invalid input

        //    RunTheRest();


        //}


        ///// <summary>
        ///// Placeholder
        ///// </summary>
        ///// <param name="dataFileLocation"></param>
        ///// <param name="numberOfDay"></param>
        ///// <param name="examsStartTime"></param>
        ///// <param name="examsLength"></param>
        ///// <param name="timeBetweenExams"></param>
        ///// <param name="lunchLength"></param>
        //public static void Run(string dataFileLocation, int numberOfDays, TimeSpan examsStartTime,
        //                                      TimeSpan examsLength, TimeSpan timeBetweenExams,
        //                                      TimeSpan lunchLength)
        //{
        //    Schedule.SetupScheduleConstraints(numberOfDays, examsStartTime, examsLength, timeBetweenExams, lunchLength);
        //    Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv"); // ? throw exceptions for invalid input

        //    RunTheRest();

        ////}

        ///// <summary>
        ///// 
        ///// </summary>
        //public static void RunTheRest()
        //{

        //    // Sort classes based on Overlapping classes ascending then by enrollment Descedending ????
        //    List<Class> sortedClasses = Schedule.AllClasses.OrderBy(c => FETP_Controller.getNumberOfOverlappingClasses(Schedule.AllClasses, c)).ThenBy(c => c.Enrollment).ToList(); // change thenby
        //    List<Block> blocks = FETP_Controller.GroupClasses(sortedClasses);



        //}
        #endregion


        #region Overloaded Operators
        //Overloaded operators here

        #endregion
    }
}
