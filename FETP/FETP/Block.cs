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
    public class Block
    {
        /**************************************************************************\
        Block - Data Members 
        \**************************************************************************/
        protected List<Class> classesInBlock;
        

        /**************************************************************************\
        Block - Properties 
        \**************************************************************************/
        public List<Class> ClassesInBlock
        {
            get { return classesInBlock; }
        }
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
        public int Average
        {
            get
            {
                if (classesInBlock != null)
                    return this.Enrollment / this.classesInBlock.Count;
                else return 0;
            }
        }
        public int Variance
        {
            get
            {
                int variance = 0;
                foreach (Class cl in this.classesInBlock)
                {
                    int difference = cl.Enrollment - this.Average;
                    variance += (difference * difference);
                }
                return variance / this.ClassesInBlock.Count;
            }
        }
        public double StandardDeviation
        {
            get
            {
                return Math.Sqrt(this.Variance);
            }
        }
        public int FitnessScore
        {
            get
            {
                int fitnessScore = 0;
                foreach(Class cl in this.classesInBlock)
                {
                    if(!this.doesClassOverlapWithBlock(cl)) // if the class does not overlap with ALL classes
                    {
                        fitnessScore += cl.Enrollment * GA_Controller.WEIGHT_OVERLAPPING_CLASSES;
                    }
                    // ? add more weighting here
                }
                return fitnessScore;
            }
        } // ? needs more work


        /**************************************************************************\
        Block - Methods 
        \**************************************************************************/

        /**************************************************************************\
        Constructor: Default 
        Description: Takes in data values and creates Block with those values
        ? need new constructor
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
        Constructor: Block 
        Description: Creates a new Block with only the in class in it
        ? need new constructor
        \**************************************************************************/
        public Block(Class inClass)
        {
            this.classesInBlock = new List<Class>();
            this.addClass(inClass);
        }


        // adds class to block and increaments time
        // only if class overlaps all other classes in block
        /**************************************************************************\
        Method: addClass
        Description: Addes class to list of classes in block. 
                     doesn't add class if the class does not overlap with group
                     ? this is maybe lowering cohesion
                     ? don't need anymore due to enrollment being a property
        \**************************************************************************/
        public void addClass(Class inClass)
        {
            classesInBlock.Add(inClass);
        }


        // returns wheter the class was found and removed// returns wheter the class was found and removed.
        /**************************************************************************\
        Method: isClassInGroup
        Description: Determines if the class is in the group
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
        Method:  
        Description: ? see addClass
        \**************************************************************************/
        //public bool removeClass(Class inClass)
        //{
        //    if (isClassInGroup(inClass))
        //    {
        //        this.enrollment -= inClass.Enrollment;
        //    }
        //    return this.classesInBlock.Remove(inClass);  // this should work
        //}

        /**************************************************************************\
        Method: doesClassOverlap
        Description: Determins if the inClass overlaps with ALL classes in block
        ?
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
        Description: Displays all informations stored in Block instance
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
        Method:  
        Description: 
        \**************************************************************************/
        public void DisplayAllClasses()
        {
            foreach (Class cl in this.classesInBlock)
                cl.Display();
        }

    } // end class Block
}
