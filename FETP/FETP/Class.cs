using System;
using System.Collections.Generic;

namespace FETP
{
    /// <summary>
    /// Contains basic information for a single class and functions to make use
    /// of data.
    /// </summary>
    [Serializable]
    public class Class
    {
        #region Utilities
        //Rationale: Utilities are supporting functions that are used often such as
        //           Random and others

        //Utility Data Members Here

        //Utility Methods Here

        #endregion


        #region Data Constants
        //Data Constants Here

        #endregion


        #region Data Members

        /// <summary>
        /// Start time of the clas 
        /// </summary>
        protected TimeSpan startTime;

        /// <summary>
        /// End time of the class
        /// </summary>
        protected TimeSpan endTime;

        /// <summary>
        /// Number of students in class
        /// </summary>
        protected int enrollment;

        /// <summary>
        /// Days that the class meets
        /// </summary>
        protected List<DayOfWeek> daysMeet;

        #endregion


        #region Properties
        /// <summary>
        /// Getter/setter for the start time of the class
        /// </summary>
        public TimeSpan StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        /// <summary>
        /// Getter/setter for the end time of the class
        /// </summary>
        public TimeSpan EndTime
        {
            get { return this.endTime; }
            set { this.startTime = value; }
        }

        /// <summary>
        /// Getter/setter for the number of students in the class
        /// </summary>
        public int Enrollment
        {
            get { return this.enrollment; }
            set { this.enrollment = value; }
        }

        /// <summary>
        /// Getter/setter for the days the class meets
        /// </summary>
        public List<DayOfWeek> DaysMeet
        {
            get { return this.daysMeet; }
            set { this.daysMeet = value; }
        }

        #endregion


        #region Methods
        /// <summary>
        /// Used for XML serialization 
        /// </summary>
        private Class()
        {

        }


        /// <summary>
        /// Takes in data values for the start time, end time, number of students enrolled, 
        /// days met and creates a class with those values.
        /// </summary>
        /// <param name="inStartTime"></param>
        /// <param name="inEndTime"></param>
        /// <param name="inEnrollment"></param>
        /// <param name="inDaysMeet"></param>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public Class(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment, List<DayOfWeek> inDaysMeet)
        {
            this.startTime = inStartTime;
            this.endTime = inEndTime;
            this.enrollment = inEnrollment;

            if (inDaysMeet == null)
            {
                throw new Exception("Class does not have any days attached. A class must have days it meets on.");
            }
            else
            {
                this.daysMeet = inDaysMeet;
            }
        }


        /// <summary>
        /// Displays all information stored in a Class instance with formatting.
        /// </summary>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public void Display()
        {
            Console.Write("Days Meet: ");
            foreach (DayOfWeek day in daysMeet)
                Console.Write("{0} ", day);

            Console.WriteLine("");
            Console.WriteLine("Start Time: {0}", this.startTime);
            Console.WriteLine("End Time: {0}", this.endTime);
            Console.WriteLine("Enrollment: {0}", this.enrollment);
        }


        //TODO: This function does not do anything. The complications of writing a hash function are not needed for the current program. Investiagate further. Required to make compiler happy with overloaded "==" operator
        /// <summary>
        /// Overloaded Hash function. It is improperly implemented due to the complexity
        /// being too high and the function will not be used. C# requires it to be overloaded
        /// if comparison is overloaded.
        /// </summary>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        //TODO: It is *possibly* improperly implemented due to the complexity being too
        //      high and the function will not be used. C# requires it to be overloaded
        //      if comparison is overloaded.
        /// <summary>
        /// Overloaded Equals function.
        /// </summary>
        /// <param name="obj"></param>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public override bool Equals(object obj)
        {
            Class inClass = obj as Class;
            if (inClass != null)
            {
                return this == inClass;
            }
            else return false;
        }

        #endregion


        #region Overloaded Operators
        //TODO: Why is there two of these? (== and Equals()) // answer: because i think deep down, they sorta do two very slighty different things.
        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        public static bool operator ==(Class class1, Class class2)
        {
            //if (class1 == null && class2 == null) return true;
            return (class1.StartTime == class2.StartTime && class1.EndTime == class2.EndTime && class1.Enrollment == class2.Enrollment && class1.DaysMeet == class2.DaysMeet); // TODO: comparing list should work, if so, remove todo
        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        //Author: Benjamin Etheredge
        //Date: 3-25-2016
        //Modifications:  
        //Date(s) Tested:
        //Approved By:
        public static bool operator !=(Class class1, Class class2)
        {
           // if (class1 == null && class2 == null) return false;
            return (class1.StartTime != class2.StartTime || class1.EndTime != class2.EndTime || class1.Enrollment != class2.Enrollment || class1.DaysMeet != class2.DaysMeet); // yay. used cs 245 to make this code faster
        }

        #endregion
    }
}
