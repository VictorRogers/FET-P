using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    public abstract class CourseInformation
    {
        protected TimeSpan startTime;
        protected TimeSpan endTime;
        protected int enrollment;

        // Accessors and Mutators
        public TimeSpan StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }
        public TimeSpan EndTime
        {
            get { return this.endTime; }
            set { this.startTime = value; }
        }
        public int Enrollment
        {
            get { return this.enrollment; }
            set { this.enrollment = value; }
        }

        public CourseInformation(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment)
        {
            this.startTime = inStartTime;
            this.endTime = inEndTime;
            this.enrollment = inEnrollment;
        }

        public virtual void Display()
        {
            Console.WriteLine("Start Time: {0}", this.startTime);
            Console.WriteLine("End Time: {0}", this.endTime);
            Console.WriteLine("Enrollment: {0}", this.enrollment);
        }

    }
}
