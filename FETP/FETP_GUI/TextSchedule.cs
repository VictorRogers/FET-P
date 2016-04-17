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
            richTextBox1.Text += "\n***************************************";
            richTextBox1.Text += "\nSCHEDULED BLOCKS INFORMATION";
            richTextBox1.Text += "\n***************************************";
            for (int i = 0; i < _schedule.Blocks.Length; i++)
            {
                if (_schedule.Blocks[i] != null)
                {

                    richTextBox1.Text += ("\n**************");
                    richTextBox1.Text += ("\nBlock");
                    richTextBox1.Text += ("\n**************");
                    richTextBox1.Text += string.Format("\nBlock start time: {0}", _schedule.StartTimesOfExams[i % _schedule.NumberOfTimeSlotsAvailablePerDay]);
                    DisplayBlock(_schedule.Blocks[i]);
                    richTextBox1.Text += ("\nClasses In Block");
                    richTextBox1.Text += ("\n********");

                    DisplayAllClasses(_schedule.Blocks[i].ClassesInBlock);
                    richTextBox1.Text += "\n";

                }
                else
                {
                    richTextBox1.Text += ("\nNOT GOOD: IN DisplayBlocks");
                }

            }

            richTextBox1.Text += ("\n*******************************************");
            richTextBox1.Text += ("\nNON SCHEDULED BLOCKS INFORMATION");
            richTextBox1.Text += ("*******************************************");
            foreach (Block block in _schedule.LeftoverBlocks)
            {
                richTextBox1.Text += ("\n**************");
                richTextBox1.Text += ("\nBlock");
                richTextBox1.Text += ("\n**************");
                DisplayBlock(block);
                richTextBox1.Text += ("\nClasses In Block");
                richTextBox1.Text += ("\n********");
                DisplayAllClasses(block.ClassesInBlock);
                richTextBox1.Text += ("\n");
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
        public void DisplayAllClasses(List<Class> classesInBlock)
        {
            foreach (Class cl in classesInBlock)
                DisplayClass(cl);
        }

        /// <summary>
        /// Displays all information stored in a Class instance with formatting.
        /// </summary>
        public void DisplayClass(Class cl)
        {
            richTextBox1.Text += "\nDays Meet: ";
            foreach (DayOfWeek day in cl.DaysMeet)
                richTextBox1.Text += string.Format("{0} ", day);

            richTextBox1.Text += string.Format("\nStart Time: {0}", cl.StartTime);
            richTextBox1.Text += string.Format("\nEnd Time: {0}", cl.EndTime);
            richTextBox1.Text += string.Format("\nEnrollment: {0}", cl.Enrollment);
            richTextBox1.Text += "\n";
        }
    }
}
