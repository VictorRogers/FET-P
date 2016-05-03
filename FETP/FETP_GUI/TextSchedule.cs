using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FETP;

namespace FETP_GUI
{
    public partial class TextSchedule : UserControl
    {
        private Schedule _schedule;
        
        /// <summary>
        /// Dynamic Constructer displays entire schedule in text format
        /// </summary>
        /// <param name="schedule">Schedule object to display</param>
        //Amy Brown
        public TextSchedule(Schedule schedule)
        {
            _schedule = schedule;

            InitializeComponent();
            Display();
            DisplayBlocks();
        }

        /// <summary>
        /// Display Schedule Constraints
        /// </summary>
        //Ben Etherege and Amy Brown
        public void Display()
        {
            richTextBox1.Text += "\n*******************************************************************************************";
            richTextBox1.Text += "\nFINAL EXAM SCHEDULE INFORMATION";
            richTextBox1.Text += "\n*******************************************************************************************";
            richTextBox1.Text += string.Format("\nNumber of Days: {0}", _schedule.NumberOfDays);
            richTextBox1.Text += string.Format("\nStart Time for Exams: {0}", _schedule.ExamsStartTime);
            richTextBox1.Text += string.Format("\nLength of Exams: {0}", _schedule.ExamsLength);
            richTextBox1.Text += string.Format("\nTime Between Exams: {0}", _schedule.TimeBetweenExams);
            richTextBox1.Text += string.Format("\nLength of Lunch Time: {0}", _schedule.LunchLength);
            richTextBox1.Text += string.Format("\nNumber of Exam Times Per Day: {0}", _schedule.NumberOfTimeSlotsAvailablePerDay);
            richTextBox1.Text += "\n*******************************************************************************************\n";
        }
        
        /// <summary>
        /// Displays all blocks in schedule
        /// </summary>
        //ben Etherege and Amy Brown
        public void DisplayBlocks()
        {
            int dayNumber = 0;

            richTextBox1.Text += "\n*******************************************************************************************";
            richTextBox1.Text += "\nSCHEDULED EXAMS";
            richTextBox1.Text += "\n*******************************************************************************************";
            richTextBox1.Text += "\nClasses which meet\t\t\tExam Time";
            for (int i = 0; i < _schedule.Blocks.Length; i++)
            {
                if (_schedule.Blocks[i] != null)
                {
                    if (_schedule.StartTimesOfExams[i%_schedule.NumberOfTimeSlotsAvailablePerDay].Equals(_schedule.ExamsStartTime))
                    {
                        dayNumber++;
                        richTextBox1.Text += "\n";
                    }
                    DisplayAllClasses(_schedule.Blocks[i].ClassesInBlock, _schedule.StartTimesOfExams[i % _schedule.NumberOfTimeSlotsAvailablePerDay], _schedule.ExamsLength, dayNumber);
                }
            }

            richTextBox1.Text += ("\n\n*******************************************************************************************");
            richTextBox1.Text += ("\nUNSCHEDULED EXAMS");
            richTextBox1.Text += ("\n*******************************************************************************************");
            foreach (Block block in _schedule.LeftoverBlocks)
            {
                DisplayAllClasses(block.ClassesInBlock);
            }
            richTextBox1.Text += "\n\n\n\n\n";
        }
        
        /// <summary>
        /// Displays all Classes in a scheduled Block
        /// </summary>
        /// <param name="classesInBlock">List of all Classes in the given Block</param>
        /// <param name="startTime">Start Time of given Block</param>
        /// <param name="examLength">Length of Exam Times</param>
        /// <param name="dayNumber">Day of given Block</param>
        //Ben Etherege and Amy Brown
        public void DisplayAllClasses(List<Class> classesInBlock, TimeSpan startTime, TimeSpan examLength, int dayNumber)
        {
            foreach (Class cl in classesInBlock)
                DisplayScheduledClass(cl, startTime, examLength, dayNumber);
        }

        /// <summary>
        /// Displays all Classes in an unscheduled Block
        /// </summary>
        /// <param name="classesInBlock">List of all Classes in the given Block</param>
        //Ben Etherege and Amy Brown
        public void DisplayAllClasses(List<Class> classesInBlock)
        {
            foreach (Class cl in classesInBlock)
                DisplayUnscheduledClass(cl);
        }

        /// <summary>
        /// Displays all information stored in a scheduled Class instance with formatting.
        /// </summary>
        /// <param name="cl">Class to display</param>
        /// <param name="startTime">Start Time of class</param>
        /// <param name="examLength">Length of Exams</param>
        /// <param name="dayNumber">Day of given Class</param>
        //Ben Etherege and Amy Brown
        public void DisplayScheduledClass(Class cl, TimeSpan startTime, TimeSpan examLength, int dayNumber)
        {
            richTextBox1.Text += "\n";
            foreach (DayOfWeek day in cl.DaysMeet)
            {
                switch (day)
                {
                    case DayOfWeek.Monday:
                        richTextBox1.Text += "M";
                        break;
                    case DayOfWeek.Tuesday:
                        richTextBox1.Text += "T";
                        break;
                    case DayOfWeek.Wednesday:
                        richTextBox1.Text += "W";
                        break;
                    case DayOfWeek.Thursday:
                        richTextBox1.Text += "R";
                        break;
                    case DayOfWeek.Friday:
                        richTextBox1.Text += "F";
                        break;
                }
            }

            richTextBox1.Text += string.Format("\t{0}", cl.StartTime);
            richTextBox1.Text += string.Format(" - {0}", cl.EndTime);
            richTextBox1.Text += string.Format("\t\t{0}", startTime);
            richTextBox1.Text += string.Format(" - {0}", startTime+examLength);
            richTextBox1.Text += string.Format("\t\tDay {0}", dayNumber);
        }

        /// <summary>
        /// Displays all information stored in an unscheduled Class instance with formatting.
        /// </summary>
        /// <param name="cl">Class to display</param>
        //Ben Etherege and Amy Brown
        public void DisplayUnscheduledClass(Class cl)
        {
            richTextBox1.Text += "\n";
            foreach (DayOfWeek day in cl.DaysMeet)
            {
                switch (day)
                {
                    case DayOfWeek.Monday:
                        richTextBox1.Text += "M";
                        break;
                    case DayOfWeek.Tuesday:
                        richTextBox1.Text += "T";
                        break;
                    case DayOfWeek.Wednesday:
                        richTextBox1.Text += "W";
                        break;
                    case DayOfWeek.Thursday:
                        richTextBox1.Text += "R";
                        break;
                    case DayOfWeek.Friday:
                        richTextBox1.Text += "F";
                        break;
                }
            }

            richTextBox1.Text += string.Format("\t{0}", cl.StartTime);
            richTextBox1.Text += string.Format(" - {0}", cl.EndTime);
        }
    }
}
