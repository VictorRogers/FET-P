using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FETP;

namespace FETP_GUI
{
    public partial class TextSchedule : UserControl
    {
        private Schedule _schedule;

        public TextSchedule()
        {
            InitializeComponent();
        }

        public TextSchedule(Schedule schedule)
        {
            _schedule = schedule;

            InitializeComponent();
            Display();
            DisplayBlocks();
        }

        public void Display()
        {
            richTextBox1.Text += "\n*******************************";
            richTextBox1.Text += "\nSCHEDULE INFORMATION";
            richTextBox1.Text += "\n*******************************";
            richTextBox1.Text += string.Format("\nNumber of Days: {0}", _schedule.NumberOfDays);
            richTextBox1.Text += string.Format("\nStart Time for Exams: {0}", _schedule.ExamsStartTime);
            richTextBox1.Text += string.Format("\nLength of Exams: {0}", _schedule.ExamsLength);
            richTextBox1.Text += string.Format("\nTime Between Exams: {0}", _schedule.TimeBetweenExams);
            richTextBox1.Text += string.Format("\nLength of Lunch Time: {0}", _schedule.LunchLength);
            richTextBox1.Text += string.Format("\nNumber of Timeslots Per Day: {0}", _schedule.NumberOfTimeSlotsAvailablePerDay);
            richTextBox1.Text += string.Format("\nNumber of Timeslots: {0}", _schedule.NumberOfTimeSlotsAvailable);
            richTextBox1.Text += "\n*******************************\n";
        }


        /// <summary>
        /// Displays all blocks in schedule
        /// </summary>
        public void DisplayBlocks()
        {
            int dayNumber = 0;

            richTextBox1.Text += "\n***************************************";
            richTextBox1.Text += "\nSCHEDULED CLASSES";
            richTextBox1.Text += "\n***************************************";
            richTextBox1.Text += "\nClasses which meet\t\t\tExam Time";
            for (int i = 0; i < _schedule.Blocks.Length; i++)
            {
                if (_schedule.Blocks[i] != null)
                {

                    //richTextBox1.Text += ("\n**************");
                    //richTextBox1.Text += ("\nGroup #" + (i + 1).ToString());
                    //richTextBox1.Text += ("\n**************");
                    //richTextBox1.Text += string.Format("\nBlock start time: {0}", _schedule.StartTimesOfExams[i % _schedule.NumberOfTimeSlotsAvailablePerDay]);
                    //DisplayBlock(_schedule.Blocks[i]);
                    //richTextBox1.Text += ("\nClasses In Block");
                    //richTextBox1.Text += ("\n********");

                    if (_schedule.StartTimesOfExams[i%_schedule.NumberOfTimeSlotsAvailablePerDay].Equals(_schedule.ExamsStartTime))
                    {
                        dayNumber++;
                        richTextBox1.Text += "\n\n";
                    }
                    DisplayAllClasses(_schedule.Blocks[i].ClassesInBlock, _schedule.StartTimesOfExams[i % _schedule.NumberOfTimeSlotsAvailablePerDay], _schedule.ExamsLength, dayNumber);
                    richTextBox1.Text += "\n";
                    //richTextBox1.Text += "\n";

                }
                else
                {
                    richTextBox1.Text += ("\nNOT GOOD: IN DisplayBlocks");
                }

            }

            richTextBox1.Text += ("\n*******************************************");
            richTextBox1.Text += ("\nNON SCHEDULED CLASSES");
            richTextBox1.Text += ("\n*******************************************");
            foreach (Block block in _schedule.LeftoverBlocks)
            {
                //richTextBox1.Text += ("\n**************");
                //richTextBox1.Text += ("\nBlock");
                //richTextBox1.Text += ("\n**************");
                //DisplayBlock(block);
                //richTextBox1.Text += ("\nClasses In Block");
                //richTextBox1.Text += ("\n********");
                DisplayAllClasses(block.ClassesInBlock);
                //richTextBox1.Text += ("\n");
            }
        }

        /// <summary>
        /// Displays all information stored in a Block instance with formatting.
        /// </summary>
        public void DisplayBlock(Block block)
        {
            richTextBox1.Text += string.Format("\nNumber of Classes in Block: {0}", block.ClassesInBlock.Count);
            richTextBox1.Text += string.Format("\nTotal Enrollment: {0}", block.Enrollment);
            richTextBox1.Text += string.Format("\nAverage Enrollment: {0}", block.Average);
            richTextBox1.Text += string.Format("\nVariance: {0}", block.Variance);
            richTextBox1.Text += string.Format("\nStandard Deviation: {0}", block.StandardDeviation);
            richTextBox1.Text += string.Format("\nWeighted Average Starting Time: {0}", block.WeightedAverageStartTime);
        }


        /// <summary>
        /// Add a description
        /// </summary>
        public void DisplayAllClasses(List<Class> classesInBlock, TimeSpan startTime, TimeSpan examLength, int dayNumber)
        {
            foreach (Class cl in classesInBlock)
                DisplayScheduledClass(cl, startTime, examLength, dayNumber);
        }

        /// <summary>
        /// Add a description
        /// </summary>
        public void DisplayAllClasses(List<Class> classesInBlock)
        {
            foreach (Class cl in classesInBlock)
                DisplayUnscheduledClass(cl);
        }

        /// <summary>
        /// Displays all information stored in a Class instance with formatting.
        /// </summary>
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
            //richTextBox1.Text += string.Format("\nEnrollment: {0}", cl.Enrollment);
            richTextBox1.Text += string.Format("\t\t{0}", startTime);
            richTextBox1.Text += string.Format(" - {0}", startTime+examLength);
            richTextBox1.Text += string.Format("\t\tDay {0}", dayNumber);
            richTextBox1.Text += "\n";
        }

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
            //richTextBox1.Text += string.Format("\nEnrollment: {0}", cl.Enrollment);
            richTextBox1.Text += "\n";
        }
    }
}
