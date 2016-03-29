using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Globalization;


namespace FETP
{
    /**************************************************************************\
    Class: FETP_Controller (Final Exam Timetabling Problem Controller)
    Description: 
    TODO: Rewrite description. The old one was out of date.
    \**************************************************************************/
    public static class FETP_Controller
    {
        #region Utilities
        /**************************************************************************\
        Class: FETP_Controller
        Section: Utilities
        \**************************************************************************/
        //Used for random number generation
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();


        /**************************************************************************\
        Utility Method: RandomFloatBetween01 
        Description: Will return a random float between 0 and 1 
        \**************************************************************************/       
        public static float RandomFloatBetween01()
        {
            lock (syncLock)
            {
                return (float)random.NextDouble();
            }
        }
        
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
        /**************************************************************************\
        Class: FETP_Controller 
        Section: Data Constants 
        \**************************************************************************/
        public const int THREAD_LIMIT = 6;

        #endregion


        #region Data Members
        /**************************************************************************\
        Class: FETP_Controller 
        Section: Data Members 
        \**************************************************************************/
        //Add data members here

        #endregion


        #region Properties
        /**************************************************************************\
        Class: FETP_Controller 
        Sections: Properties
        TODO: Let me know if headers for properties are too excessive (VR)
        \**************************************************************************/
        /**************************************************************************\
        Property: ExampleProperty 
        Description: This is an example
        TODO: Delete this if an actual property is added 
        \**************************************************************************/

        #endregion


        #region Methods
        /**************************************************************************\
        Class: FETP_Controller
        Section: Methods
        \**************************************************************************/
        /**************************************************************************\
        Method: ComputeVarianceOfBlocks
        Description: 
        \**************************************************************************/
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
        Method: GroupClass
        Description: Takes in a list of class groups and a class. The method then
                     returns a new list of class groups with the class inserted
                     into the first possible group.
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


        /**************************************************************************\
        Method: GroupClass
        Description: Takes in a list of class groups and a class. The method then
                     returns a new list of class groups with the class inserted
                     into the first possible group.
        \**************************************************************************/
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


        /**************************************************************************\
        Method: CoalesceClassesTogether 
        Description: 
        TODO: Add a description
        \**************************************************************************/
        public static List<Block> CoalesceClassesTogether(List<Class> classes)
        {
            List<Block> classestobegrouped = new List<Block>(); // variable to contain the list of all grouped classes

            foreach (Class cl in classes)
            {
                classestobegrouped = GroupClass(classestobegrouped, cl); // TODO: clean this up
            }
            return classestobegrouped;
        }


        /**************************************************************************\
        Method: removeClass
        Description: 
        TODO: Add a description
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
        /**************************************************************************\
        Method: Run 
        Description: 
        TODO: Add a description
        \**************************************************************************/
        public static void Run(string dataFileLocation, string constraintsFileLocation)
        {
            Schedule.readInputConstraintsFile("../../../../Example Data/Ben Made Constraints Sample.txt");
            Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv"); // ? throw exceptions for invalid input
            
            // Sort classes based on Overlapping classes ascending then by enrollment Descedending ????
            List < Class > sortedClasses = Schedule.AllClasses.OrderBy(c => FETP_Controller.getNumberOfOverlappingClasses(Schedule.AllClasses, c)).ThenBy(c => c.Enrollment).ToList(); // change thenby
            List < Block > blocks = FETP_Controller.GroupClasses(sortedClasses);
            
            Schedule schedule = new Schedule();
        }

        #endregion


        #region Overloaded Operators
        /**************************************************************************\
        Class: FETP_Controller 
        Section: Overloaded Operators 
        \**************************************************************************/
        /**************************************************************************\
        Operator: ==
        Description: This is an example 
        \**************************************************************************/

        #endregion
    }
}
