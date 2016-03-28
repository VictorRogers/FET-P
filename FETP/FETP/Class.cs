using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    /**************************************************************************\
    Class: Class
    Description: Contains basic information for a single class and functions 
                 to make use of data
    \**************************************************************************/ 
    public class Class
    {
        /**************************************************************************\
        Class: Class
        Section: Data Members 
        \**************************************************************************/
        protected TimeSpan startTime;
        protected TimeSpan endTime;
        protected int enrollment;
        protected List<DayOfWeek> daysMeet;


        /**************************************************************************\
        Class: Class
        Section: Properties
        TODO: Let me know if headers for properties are too excessive (VR)
        \**************************************************************************/
        /**************************************************************************\
        Property: StartTime
        Description:
        TODO: Add a description
        \**************************************************************************/
        public TimeSpan StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }


        /**************************************************************************\
        Property: EndTime 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public TimeSpan EndTime
        {
            get { return this.endTime; }
            set { this.startTime = value; }
        }


        /**************************************************************************\
        Property: Enrollment 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public int Enrollment
        {
            get { return this.enrollment; }
            set { this.enrollment = value; }
        }


        /**************************************************************************\
        Property: DaysMeet 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public List<DayOfWeek> DaysMeet
        {
            get { return this.daysMeet; }
            set { this.daysMeet = value; }
        }


        /**************************************************************************\
        Class: Class
        Section: Methods 
        \**************************************************************************/
        /**************************************************************************\
        Constructor: Default 
        Description: Takes in data values and creates class with those values
        TODO: Change name to Overloaded constructor in header?
        \**************************************************************************/
        public Class(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment, List<DayOfWeek> inDaysMeet)
        {
            this.startTime = inStartTime;
            this.endTime = inEndTime;
            this.enrollment = inEnrollment;

            if (inDaysMeet == null)
            {
                //inDaysMeet = new List<DayOfWeek>(); // TODO: bad
                throw new Exception("Class does not have any days attached. A class must have days it meets on.");
            }
            else
            {
                this.daysMeet = inDaysMeet;
            }
        }


        /**************************************************************************\
        Method: Display
        Description: Displays all informations stored in Class instance
                     with formatting.
        \**************************************************************************/
        public void Display()
        {
            Console.Write("Days Meet: ");
            foreach (DayOfWeek day in daysMeet)
                Console.Write("{0} ", day);

            Console.WriteLine("");
            Console.WriteLine("Start Time: {0}", this.startTime);
            Console.WriteLine("End Time: {0}", this.endTime);
            Console.WriteLine("Enrollment: {0}", this.enrollment);
            Console.WriteLine("");
        }


        /**************************************************************************\
        Method: GetHashCode
        Description: Overloaded Hash function. It is improperly implemented 
                     due to the complexity being too high and the function
                     will not be used. C# requires it to be overloaded if
                     comparison is overloaded.
        // TODO: this function does not do anything. The complications of writing a hash function are not needed for current program
        \**************************************************************************/
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /**************************************************************************\
        Method: Equals
        Description: Overloaded Equals function. It is *possibly* improperly  
                     implemented due to the complexity being too high and the 
                     function will not be used. C# requires it to be overloaded 
                     if comparison is overloaded.
        \**************************************************************************/
        public override bool Equals(object obj)
        {
            Class inClass = obj as Class;
            if (inClass != null)
            {
                return this == inClass;
            }
            else return false;
        }


        /**************************************************************************\
        Class: Class
        Section: Overloaded Operators 
        \**************************************************************************/
        /**************************************************************************\
        Operator: == 
        Description: Should take a chromosome as an input and output its fitness
                     score.
        TODO: Remove ambiguity between overloaded == and Equals()
        \**************************************************************************/
        public static bool operator ==(Class class1, Class class2)
        {
            //if (class1 == null && class2 == null) return true;
            return (class1.StartTime == class2.StartTime && class1.EndTime == class2.EndTime && class1.Enrollment == class2.Enrollment && class1.DaysMeet == class2.DaysMeet); // TODO: comparing list should work
        }


        /**************************************************************************\
        Operator: !=
        Description: Should take a chromosome as an input and output its fitness
                     score.
        \**************************************************************************/
        public static bool operator !=(Class class1, Class class2)
        {
           // if (class1 == null && class2 == null) return false;
            return (class1.StartTime != class2.StartTime || class1.EndTime != class2.EndTime || class1.Enrollment != class2.Enrollment || class1.DaysMeet != class2.DaysMeet); // yay. used cs 245 to make this code faster
        }
    }
}
