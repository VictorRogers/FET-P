﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    // Programmer: Ben and Vic
    public class Class : CourseInformation
    {


        protected List<DayOfWeek> daysMeet;


        public List<DayOfWeek> DaysMeet
        {
            get { return this.daysMeet; }
            set { this.daysMeet = value; }
        }

        // Intializes Class
        public Class(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment, List<DayOfWeek> inDaysMeet)
            : base(inStartTime, inEndTime, inEnrollment)
        {

            if (inDaysMeet == null)
            {
                inDaysMeet = new List<DayOfWeek>(); // ? bad
            }
            else
            {

                this.daysMeet = inDaysMeet;
            }
        }

        public override void Display()
        {
            Console.Write("Days Meet: ");
            foreach (DayOfWeek day in daysMeet)
            {
                Console.Write("{0} ", day);
            }
            Console.WriteLine("");

            Console.WriteLine("Start Time: {0}", this.startTime);
            Console.WriteLine("End Time: {0}", this.endTime);
            Console.WriteLine("Enrollment: {0}", this.enrollment);

            Console.WriteLine("");
        }

        // ? this function does not do anything. The complications of writing a hash function is not needed for current program
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override bool Equals(object obj)
        {
            Class inClass = obj as Class;
            if (inClass != null)
            {
                return this == inClass;
            }
            else return false;
        }

        /*
        public bool Equals(Class inClass)
        {
            return (this.StartTime == inClass.StartTime && this.EndTime == inClass.EndTime && this.Enrollment == inClass.Enrollment && this.DaysMeet == inClass.DaysMeet);
        }
        */


        public static bool operator ==(Class class1, Class class2)
        {
            //if (class1 == null && class2 == null) return true;
            return (class1.StartTime == class2.StartTime && class1.EndTime == class2.EndTime && class1.Enrollment == class2.Enrollment && class1.DaysMeet == class2.DaysMeet); // ? comparing list should work
        }

        public static bool operator !=(Class class1, Class class2)
        {
           // if (class1 == null && class2 == null) return false;
            return (class1.StartTime != class2.StartTime || class1.EndTime != class2.EndTime || class1.Enrollment != class2.Enrollment || class1.DaysMeet != class2.DaysMeet); // yay. used cs 245 to make this code faster
        }

    } // end class Class
}
