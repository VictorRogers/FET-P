using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    // block contains classes that overlapped and were coalesced
    public class Block : CourseInformation
    {
        protected List<Class> classesInBlock;

        public List<Class> ClassesInBlock
        {
            get { return classesInBlock; }
        }

        public int Average
        {
            get
            {
                if (classesInBlock != null)
                    return this.enrollment / this.classesInBlock.Count;
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



        /*
        public Block(TimeSpan inStartTime, TimeSpan inEndTime, List<DayOfWeek> inDaysMeet, List<Class> inClasses = null)
            : base(inStartTime, inEndTime, inDaysMeet, )
        {
            this.classesInBlock = inClasses;

            // calculate total enrollement
            foreach (Class clas in inClasses)
                this.enrollment += clas.Enrollment;

        }
        */
        public Block(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment, List<Class> inClasses)
            : base(inStartTime, inEndTime, inEnrollment)
        {
            this.classesInBlock = inClasses;
        }

        //public Block(Class inClass, List<Class> inClasses = null)
        //    : base(inClass.StartTime, inClass.EndTime, 0)
        //{
        //    this.classesInBlock = inClasses;
        //    addClass(inClass);
        //}

        // adds class to block and increaments time
        public void addClass(Class inClass)
        {
            if (this.classesInBlock == null)
                this.classesInBlock = new List<Class>();

            classesInBlock.Add(inClass);
            this.enrollment += inClass.Enrollment;
        }

        // returns wheter the class was found and removed
        public bool removeClass(Class inClass)
        {
            bool isFound = false;
            int i = 0;
            while (i < classesInBlock.Count && !isFound)
            {
                if (this.classesInBlock[i] == inClass)
                {
                    this.ClassesInBlock.RemoveAt(i);
                    enrollment -= inClass.Enrollment;
                    isFound = true;
                }
            }
            return isFound;
        }



        public override void Display()
        {
            // ??Console.WriteLine("Number of Classes in Block: {0}", this.classesInBlock.Count);
            Console.WriteLine("Average Enrollment: {0}", this.Average);
            Console.WriteLine("Variance: {0}", this.Variance);
            Console.WriteLine("Standard Deviation: {0}", this.StandardDeviation);
            base.Display();
        }

        public void DisplayAllClasses()
        {
            foreach (Class cl in this.classesInBlock)
                cl.Display();
        }

    } // end class Block
}
