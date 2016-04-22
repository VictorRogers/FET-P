﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FETP;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FETP_GUI
{
    public partial class FETP_Form : Form
    {
        private static int NUMBER_OF_EXAMS_PER_DAY;// = 10;
        private static int NUMBER_OF_EXAMS; //= NUMBER_OF_DAYS * NUMBER_OF_EXAMS_PER_DAY;

        int daysNum;
        int beginTime;
        int examLength;
        int breakLength;
        int lunchLength;

        string constraintsFile;
        string enrollmentFile;

        Schedule schedule;

        Dictionary<string, UserControl> views;

        DataCollection dataCollection1;
        SchedulePresenter scheduleView;

        FullCalendar fullCal;
        SingleDayCalendar miniCal;
        TextSchedule textCal;

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// FETP_Form Constructor
        /// </summary>
        public FETP_Form()
        {
            InitializeComponent();

            views = new Dictionary<string, UserControl>();

            //auth1.Login += new Auth.LoginClickHandler(Login);
        }

        //------------------------------------------------------------------------------------------

        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        public void Login(object sender, EventArgs e)
        {
            bool isValid = false;
            string userName = GetLoggedInUserName();

            if (userName.ToLowerInvariant().Contains(auth1.txtUserName.Text.Trim().ToLowerInvariant()) &&
                    userName.ToLowerInvariant().Contains(auth1.txtDomain.Text.Trim().ToLowerInvariant()))
            {
                isValid = IsValidCredentials(auth1.txtUserName.Text.Trim(), auth1.txtPwd.Text.Trim(), auth1.txtDomain.Text.Trim());
            }

            if (isValid)
            {
                Controls.Clear();
                Size = new Size(355, 401);

                dataCollection1 = new DataCollection();
                Controls.Add(dataCollection1);

                dataCollection1.Dock = DockStyle.Fill;
                dataCollection1.Location = new Point(0, 0);
                dataCollection1.Margin = new Padding(10, 9, 10, 9);
                dataCollection1.Name = "dataCollection1";
                dataCollection1.Size = new Size(335, 334);
                dataCollection1.TabIndex = 0;
                dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
                dataCollection1.ClearForm += new DataCollection.ClearClickHandler(ClearAllTextBoxes);
            }
            else
            {
                MessageBox.Show("Invalid Windows username or password.");
            }
        }

        private string GetLoggedInUserName()
        {
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            return currentUser.Name;
        }

        private bool IsValidCredentials(string userName, string password, string domain)
        {
            if (domain == "")
            {
                domain = Environment.MachineName;
            }

            IntPtr tokenHandler = IntPtr.Zero;
            bool isValid = LogonUser(userName, domain, password, 2, 0, ref tokenHandler);
            return isValid;
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
        /// Using information from the DataCollection form's text boxes, generate and present a schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GenerateFullSchedule(object sender, EventArgs e)
        {
            bool isSchedulePossible = true;
            bool isDaysValid = true;
            bool isBeginValid = true;
            bool isExamValid = true;
            bool isBreakValid = true;
            bool isLunchValid = true;

            //Get data from form
            //using immediate if to get around empty text box exceptions for now - defaults all values to 1
            daysNum = (dataCollection1.days_textBox.Text.Equals(string.Empty)) ? -1 : Convert.ToInt32(dataCollection1.days_textBox.Text);
            beginTime = (dataCollection1.startTime_textBox.Text.Equals(string.Empty)) ? -1 : Convert.ToInt32(dataCollection1.startTime_textBox.Text);
            examLength = (dataCollection1.examLength_textBox.Text.Equals(string.Empty)) ? -1 : Convert.ToInt32(dataCollection1.examLength_textBox.Text);
            breakLength = (dataCollection1.breakLength_textBox.Text.Equals(string.Empty)) ? -1 : Convert.ToInt32(dataCollection1.breakLength_textBox.Text);
            lunchLength = (dataCollection1.lunchLength_textBox.Text.Equals(string.Empty)) ? -1 : Convert.ToInt32(dataCollection1.lunchLength_textBox.Text);

            constraintsFile = (dataCollection1.scheduleBrowse_textBox.Text.Equals(string.Empty)) ? string.Empty : dataCollection1.scheduleBrowse_textBox.Text;
            enrollmentFile = (dataCollection1.enrollmentBrowse_textBox.Text.Equals(string.Empty)) ? string.Empty : dataCollection1.enrollmentBrowse_textBox.Text;

            //Check 5 ints for valid ranges
            if (constraintsFile.Equals(string.Empty))
            {
                if (daysNum < 1 || daysNum > 7)
                {
                    isSchedulePossible = false;
                    isDaysValid = false;
                }
                if (beginTime < 7 || beginTime > 16)
                {
                    isSchedulePossible = false;
                    isBeginValid = false;
                }
                if (examLength < 0090 || examLength > 0120)
                {
                    isSchedulePossible = false;
                    isExamValid = false;
                }
                if (breakLength < 0010 || breakLength > 0030)
                {
                    isSchedulePossible = false;
                    isBreakValid = false;
                }
                if (lunchLength < 0 || lunchLength > 60)
                {
                    isSchedulePossible = false;
                    isLunchValid = false;
                }
            }

            if (isSchedulePossible)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                panel1.Controls.Clear();

                if (constraintsFile.Equals(string.Empty))
                {
                    //Convert time ints to Timespan objects
                    TimeSpan examsStartTime = TimeSpan.FromHours(beginTime);    //TODO: I think this one is wrong.
                    TimeSpan examsLength = TimeSpan.FromMinutes(examLength);
                    TimeSpan timeBetweenExams = TimeSpan.FromMinutes(breakLength);
                    TimeSpan lunchSpan = TimeSpan.FromMinutes(lunchLength);
                    //Using these 5 ints + enrollmentData file:
                    //Create schedule data structure
                    schedule = new Schedule(enrollmentFile, daysNum, examsStartTime, examsLength, timeBetweenExams, lunchSpan);
                }
                else
                {
                    //Create a schedule using the input files
                    schedule = new Schedule(enrollmentFile, constraintsFile);
                }


                //Using schedule data strucutre::
                
                //TODO: Generating this presenter neeeds to be split off into its own function
                //SchedulePresenter Constructor builds SplitContainer base presenter - container for different Schedule Views
                //This will need the Schedule data structure as a parameter
                //uses NUMBER_OF_DAYS and NUMBER_OF_EXAMS_PER_DAY from the Schedule data structure
                scheduleView = new SchedulePresenter(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay);
                scheduleView.Dock = DockStyle.Fill;

                //If the views Dictionary contains view presenters for a different Schedule object, get rid of them.
                //You should have saved the schedule if you wanted to keep them.
                views.Clear();

                //FullCalendar constructor dynamically builds drag-and-drop button matrix
                //This will need the Schedule data structure as a parameter
                //uses daysNum, examsPerDay, examLength, breakLength, and lunchLength from Schedule data structure
                fullCal = new FullCalendar(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay, examLength, breakLength, lunchLength);
                fullCal.Dock = DockStyle.Fill;

                panel1.Controls.Add(scheduleView);
                scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
                views.Add("Full", fullCal);
                Size = new Size(681, 492);
                MaximizeBox = true;
                viewToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                fullScheduleToolStripMenuItem.Enabled = false;
            }
            else
            {
                string errorMessage = string.Empty;

                if (!isDaysValid)
                {
                    errorMessage += "Invalid number of days - enter a whole number 1 - 7.";
                }
                if (!isBeginValid)
                {
                    errorMessage += "\nInvalid start time - enter a whole number 7 or greater.";
                }
                if (!isExamValid)
                {
                    errorMessage += "\nInvalid exam length - enter a whole number 90 - 120.";
                }
                if (!isBreakValid)
                {
                    errorMessage += "\nInvalid break length - enter a whole number 10 - 30.";
                }
                if (!isLunchValid)
                {
                    errorMessage += "\nInvalid lunch length - enter a whole number 0 - 60.";
                }

                MessageBox.Show(errorMessage);
            }
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DAT-File | *.dat";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = File.OpenRead(openFileDialog.FileName);
                BinaryFormatter formatter = new BinaryFormatter();
                schedule = (Schedule)formatter.Deserialize(stream);
                stream.Close();

                //TODO: This code needs to be in its own function
                FormBorderStyle = FormBorderStyle.Sizable;
                panel1.Controls.Clear();
                scheduleView = new SchedulePresenter(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay);
                scheduleView.Dock = DockStyle.Fill;
                views.Clear();
                fullCal = new FullCalendar(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay, examLength, breakLength, lunchLength);
                fullCal.Dock = DockStyle.Fill;
                panel1.Controls.Add(scheduleView);
                scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
                views.Add("Full", fullCal);
                Size = new Size(681, 492);
                MaximizeBox = true;
                viewToolStripMenuItem.Enabled = true;
                fullScheduleToolStripMenuItem.Enabled = false;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DAT-File | *.dat";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                schedule.SaveSchedule(saveFileDialog.FileName);
            }
        }

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
                fullCal = new FullCalendar(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay, examLength, breakLength, lunchLength);
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
                miniCal = new SingleDayCalendar(schedule.NumberOfDays, schedule.NumberOfTimeSlotsAvailablePerDay, examLength, breakLength, lunchLength);
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
                textCal = new TextSchedule(schedule);
                views.Add("Text", textCal);
            }

            textCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(textCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = false;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string locationToSavePdf = Path.Combine(Path.GetTempPath(), "HelpManual.pdf");
            File.WriteAllBytes(locationToSavePdf, Properties.Resources.HelpManual);
            Process.Start(locationToSavePdf);
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
