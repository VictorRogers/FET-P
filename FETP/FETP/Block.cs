using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    ///<summary>
    ///Contains grouped classes and functions to get information on
    ///properties of all classes in block.
    ///</summary>
    public class Block
    {
        #region Utilities
        //Rationale: Utilities are supporting functions that are used often such as
        //           Random and others

        //Utility Data Members Here

        //Utility Methods Here

        #endregion


        #region Data Constants
        //Add data constants here

        #endregion


        #region Data Members
        //TODO: This seems ambiguous - There is a member and a property for this
        /// <summary>
        /// Add a description
        /// </summary>
        private List<Class> classesInBlock;

        #endregion


        #region Properties
        /// <summary>
        /// Getter for classes in block
        /// </summary>
        public List<Class> ClassesInBlock
        {
            get { return classesInBlock; }
        }

        /// <summary>
        /// Getter for total enrollment of block, adds up the enrollment of each class in block
        /// </summary>
        public int Enrollment
        {
            get
            {
                int enrollment = 0;
                foreach (Class cl in classesInBlock)
                    enrollment += cl.Enrollment;
                return enrollment;
            }
        }

        /// <summary>
        /// Calculates and returns the average enrollment of all the classes in the block
        /// </summary>
        public int Average
        {
            get
            {
                if (classesInBlock != null && classesInBlock.Count != 0)
                {
                    return this.Enrollment / this.classesInBlock.Count;
                }

                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Calculates and returns the variance of enrollment across all classes
        /// </summary>
        public int Variance
        {
            get
            {
                int variance = 0;
                if(classesInBlock != null && classesInBlock.Count != 0)
                {
                    foreach (Class cl in this.classesInBlock)
                    {
                        int difference = cl.Enrollment - this.Average;
                        variance += (difference * difference);
                    }
                    variance /= this.ClassesInBlock.Count;
                }
                
                    return variance;
            }
        }

        /// <summary>
        /// Calculates and returns the standard deviation of enrollment accross all classes
        /// </summary>
        public double StandardDeviation
        {
            get
            {
                return Math.Sqrt(this.Variance);
            }
        }

        /// <summary>
        /// Determines if there are any classes in the block that do not conflict
        /// </summary>
        public bool AreThereAnyNonOverlappingClasses
        {
            get
            {
                foreach (Class cl in this.classesInBlock)
                {
                    if(!this.doesClassOverlapWithBlock(cl))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Calculates and returns the average starting time of all classes in block weighted by enrollment
        /// </summary>
        public TimeSpan WeightedAverageStartTime 
        {
            get
            {
                TimeSpan weightedAverage = TimeSpan.Zero;
                long totalTicks = 0;

                foreach (Class cl in this.ClassesInBlock)
                {
                    totalTicks += (cl.StartTime.Ticks * cl.Enrollment);
                }

                return TimeSpan.FromTicks(totalTicks / this.Enrollment);
            }

        }

        #endregion


        #region Methods
        /// <summary>
        /// Used for XML serialization
        /// </summary>
        private Block()
        {

        }


        //TODO: Need a new constructor
        /// <summary>
        /// Takes in data values and creates Block with those values
        /// </summary>
        /// <param name="inClasses">List of classes to start block with</param>
        public Block(List<Class> inClasses = null)
        {
            if (inClasses == null)
            {
                this.classesInBlock = new List<Class>();
            }
            else {
                this.classesInBlock = new List<Class>(inClasses);
            }
        }


        //TODO: Looks like one of these overloaded constructors needs to go 
        /// <summary>
        /// Creates a new Block with only the input class in it
        /// </summary>
        /// <param name="inClass"></param>
        public Block(Class inClass)
        {
            this.classesInBlock = new List<Class>();
            this.addClass(inClass);
        }


        //TODO: this is maybe lowering cohesion
        //TODO: don't need anymore due to enrollment being a property
        /// <summary>
        /// Adds class to list of classes in block. Doesn't add class if the class
        /// does not overlap with group.
        /// </summary>
        /// <param name="inClass"></param>
        public void addClass(Class inClass)
        {
            classesInBlock.Add(inClass);
        }


        /// <summary>
        /// Determines if the class is in the group, and then returns a bool based
        /// on if the class was found and removed successfully.
        /// </summary>
        /// <param name="inClass">Class to be compared against other classes already in block</param>
        /// <returns></returns>
        public bool isClassInGroup(Class inClass)
        {
            foreach (Class cl in this.classesInBlock)
            {
                if (cl == inClass) return true;
            }
            return false;
        }


        /// <summary>
        /// Determines if the inClass overlaps with ALL classes in the block
        /// </summary>
        /// <param name="inClass">Class to compare against other classes already in block</param>
        /// <returns></returns>
        public bool doesClassOverlapWithBlock(Class inClass)
        {
            foreach (Class cl in this.classesInBlock)
            {
                if (!FETP_Controller.doClassesOverlap(cl, inClass))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Displays all information stored in a Block instance with formatting.
        /// </summary>
        public void Display()
        {
            Console.WriteLine("Number of Classes in Block: {0}", this.classesInBlock.Count);
            Console.WriteLine("Total Enrollment: {0}", this.Enrollment);
            Console.WriteLine("Average Enrollment: {0}", this.Average);
            Console.WriteLine("Variance: {0}", this.Variance);
            Console.WriteLine("Standard Deviation: {0}", this.StandardDeviation);
            Console.WriteLine("Weighted Average Starting Time: {0}", this.WeightedAverageStartTime);
        }


        /// <summary>
        /// Add a description
        /// </summary>
        public void DisplayAllClasses()
        {
            foreach (Class cl in this.classesInBlock)
                cl.Display();
        }

        #endregion


        #region Overloaded Operators

        #endregion
    }
}
