using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    /**************************************************************************\
    Class: Block (Groups of Classes)
    Description: Contains grouped classes and functions to get information
                 on properties of all classes in block 
    \**************************************************************************/
    //<summary>
    //thi is this thing
    //</summary>
    public class Block
    {
        /**************************************************************************\
        Block - Data Members 
        \**************************************************************************/
        protected List<Class> classesInBlock;
        

        /**************************************************************************\
        Block - Properties 
        TODO: Let me know if headers for properties are too excessive (VR)
        \**************************************************************************/
        /**************************************************************************\
        Property: ClassesInBlock
        Description:
        TODO: Add a description
        \**************************************************************************/
        public List<Class> ClassesInBlock
        {
            get { return classesInBlock; }
        }
        

        /**************************************************************************\
        Property: Enrollment 
        Description:
        TODO: Add a description
        \**************************************************************************/
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

        
        /**************************************************************************\
        Property: Average
        Description:
        TODO: Add a description
        \**************************************************************************/
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


        /**************************************************************************\
        Property: Variance 
        Description:
        TODO: Add a description
        \**************************************************************************/
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


        /**************************************************************************\
        Property: StandardDeviation
        Description:
        TODO: Add a description
        \**************************************************************************/
        public double StandardDeviation
        {
            get
            {
                return Math.Sqrt(this.Variance);
            }
        }


        /**************************************************************************\
        Property: FitnessScore 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public double FitnessScore
        {
            get
            {
                double fitnessScore = 0;
                foreach(Class cl in this.classesInBlock)
                {
                    if(!this.doesClassOverlapWithBlock(cl)) // if the class does not overlap with ALL classes
                    {
                        fitnessScore += cl.Enrollment * GA_Controller.WEIGHT_OVERLAPPING_CLASSES;
                    }
                    // TODO: add more weighting here
                }
                return 1/ (1 + fitnessScore);
            }
        } // TODO: needs more work


        /**************************************************************************\
        Property: AreThereAnyNonOverlappingClasses 
        Description:
        TODO: Add a description
        \**************************************************************************/
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


        /**************************************************************************\
        Block - Methods 
        \**************************************************************************/
        /**************************************************************************\
        Constructor: Default 
        Description: Takes in data values and creates Block with those values
        TODO: need new constructor
        \**************************************************************************/
        public Block(List<Class> inClasses = null)
        {
            if (inClasses == null)
            {
                this.classesInBlock = new List<Class>();
            }
            else {
                this.classesInBlock = inClasses;
            }
        }


        /**************************************************************************\
        Constructor: Overloaded 
        Description: Creates a new Block with only the input class in it
        TODO: need new constructor
        \**************************************************************************/
        public Block(Class inClass)
        {
            this.classesInBlock = new List<Class>();
            this.addClass(inClass);
        }


        /**************************************************************************\
        Method: addClass
        Description: Adds class to list of classes in block. 
                     doesn't add class if the class does not overlap with group
                     TODO: this is maybe lowering cohesion
                     TODO: don't need anymore due to enrollment being a property
        \**************************************************************************/
        public void addClass(Class inClass)
        {
            classesInBlock.Add(inClass);
        }


        /**************************************************************************\
        Method: isClassInGroup
        Description: Determines if the class is in the group, and then returns a
                     bool based on if the class was found and removed successfuly.
        \**************************************************************************/
        public bool isClassInGroup(Class inClass)
        {
            foreach (Class cl in this.classesInBlock)
            {
                if (cl == inClass) return true;
            }
            return false;
        }


        /**************************************************************************\
        Method: doesClassOverlap
        Description: Determines if the inClass overlaps with ALL classes in block
        TODO: 
        \**************************************************************************/
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


        /**************************************************************************\
        Method: Display
        Description: Displays all information stored in a Block instance
                     with formatting.
        \**************************************************************************/
        public void Display()
        {
            Console.WriteLine("Number of Classes in Block: {0}", this.classesInBlock.Count);
            Console.WriteLine("Total Enrollment: {0}", this.Enrollment);
            Console.WriteLine("Average Enrollment: {0}", this.Average);
            Console.WriteLine("Variance: {0}", this.Variance);
            Console.WriteLine("Standard Deviation: {0}", this.StandardDeviation);
        }


        /**************************************************************************\
        Method: DisplayAllClasses
        Description: 
        TODO: Add description
        \**************************************************************************/
        public void DisplayAllClasses()
        {
            foreach (Class cl in this.classesInBlock)
                cl.Display();
        }
    }
}
