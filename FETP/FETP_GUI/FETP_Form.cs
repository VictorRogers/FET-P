using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FETP_GUI
{
    public partial class FETP_Form : Form
    {
        private static int NUMBER_OF_EXAMS_PER_DAY = 10;
        private static int NUMBER_OF_EXAMS; //= NUMBER_OF_DAYS * NUMBER_OF_EXAMS_PER_DAY;

        int daysNum;
        int beginTime;
        int examLength;
        int breakLength;
        int lunchLength;

        SchedulePresenter scheduleView;
        FullCalendar fullCal;
        SingleDayCalendar miniCal;
        TextSchedule textCal;
        
        Dictionary<string, UserControl> views;


        /// <summary>
        /// FETP_Form Constructor
        /// </summary>
        public FETP_Form()
        {
            InitializeComponent();

            views = new Dictionary<string, UserControl>();

            dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
            dataCollection1.ClearForm += new DataCollection.ClearClickHandler(ClearAllTextBoxes);
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// Clear values from all text boxes in Data Collection Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClearAllTextBoxes(object sender, EventArgs e)
        {
            dataCollection1.days_textBox.Text = string.Empty;
            dataCollection1.startTime_textBox.Text = string.Empty;
            dataCollection1.examLength_textBox.Text = string.Empty;
            dataCollection1.breakLength_textBox.Text = string.Empty;
            dataCollection1.lunchLength_textBox.Text = string.Empty;
            dataCollection1.scheduleBrowse_textBox.Text = string.Empty;
            dataCollection1.enrollmentBrowse_textBox.Text = string.Empty;
        }

        /// <summary>
        /// Using information from the form's text boxes, generate and present a schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GenerateFullSchedule(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            panel1.Controls.Clear();

            //Get data from form

                //using immediate if to get around empty text box exceptions for now - defaults all values to 1
            daysNum = (dataCollection1.days_textBox.Text.Equals(string.Empty)) ? 1 : Convert.ToInt32(dataCollection1.days_textBox.Text);
            beginTime = (dataCollection1.startTime_textBox.Text.Equals(string.Empty)) ? 1 : Convert.ToInt32(dataCollection1.startTime_textBox.Text);
            examLength = (dataCollection1.examLength_textBox.Text.Equals(string.Empty)) ? 1 : Convert.ToInt32(dataCollection1.examLength_textBox.Text);
            breakLength = (dataCollection1.breakLength_textBox.Text.Equals(string.Empty)) ? 1 : Convert.ToInt32(dataCollection1.breakLength_textBox.Text);
            lunchLength = (dataCollection1.lunchLength_textBox.Text.Equals(string.Empty)) ? 1 : Convert.ToInt32(dataCollection1.lunchLength_textBox.Text);

            //Check 5 ints for valid ranges

            //Using these 5 ints + enrollmentData file:
            //Create schedule data structure

            //Using schedule data strucutre::

            //SchedulePresenter Constructor builds SplitContainer base presenter - container for different Schedule Views
            //This will need the Schedule data structure as a parameter
            //uses NUMBER_OF_DAYS and NUMBER_OF_EXAMS_PER_DAY from the Schedule data structure
            scheduleView = new SchedulePresenter(daysNum, NUMBER_OF_EXAMS_PER_DAY);
            scheduleView.Dock = DockStyle.Fill;

            //If the views Dictionary contains view presenters for a different Schedule object, get rid of them.
            //You should have saved the schedule if you wanted to keep them.
            views.Clear();

            //FullCalendar constructor dynamically builds drag-and-drop button matrix
            //This will need the Schedule data structure as a parameter
            //uses daysNum, examsPerDay, examLength, breakLength, and lunchLength from Schedule data structure
            fullCal = new FullCalendar(daysNum, NUMBER_OF_EXAMS_PER_DAY, examLength, breakLength, lunchLength);
            fullCal.Dock = DockStyle.Fill;

            panel1.Controls.Add(scheduleView);
            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            views.Add("Full", fullCal);
            Size = new Size(681, 492);
            MaximizeBox = true;
            viewToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
        }

        //------------------------------------------------------------------------------------------

        private void newScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Size = new Size(355, 401);
            panel1.Controls.Clear();

            dataCollection1 = new DataCollection();
            dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
            dataCollection1.ClearForm += new DataCollection.ClearClickHandler(ClearAllTextBoxes);

            panel1.Controls.Add(dataCollection1);
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// Display the Full Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fullScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            if (views.Keys.Contains("Full"))
            {
                fullCal = (FullCalendar)views["Full"];
            }
            else
            {
                fullCal = new FullCalendar(daysNum, NUMBER_OF_EXAMS_PER_DAY, examLength, breakLength, lunchLength);
                views.Add("Full", fullCal);
            }

            fullCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Display the Single Day Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oneDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            if (views.Keys.Contains("Single"))
            {
                miniCal = (SingleDayCalendar)views["Single"];
                views.Add("Single", miniCal);
            }
            else
            {
                //This will need the Schedule data structure as a parameter
                miniCal = new SingleDayCalendar(daysNum, NUMBER_OF_EXAMS_PER_DAY, examLength, breakLength, lunchLength);
            }

            miniCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(miniCal);
            oneDayToolStripMenuItem.Enabled = false;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Display the Text Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            if (views.Keys.Contains("Text"))
            {
                textCal = (TextSchedule)views["Text"];
            }
            else
            {
                //This will need the Schedule data structure as a parameter
                //Prints entire schedule data structure in agreed format
                textCal = new TextSchedule();
                views.Add("Text", textCal);
            }

            textCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(textCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = false;
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// While resizing the SchedulePresenter, keep the splitter in the correct position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FETP_Form_Resize(object sender, EventArgs e)
        {
            if (scheduleView != null)
            {
                scheduleView.splitContainer1.SplitterDistance = scheduleView.splitContainer1.Width - 221;
            }
        }
    }
}
