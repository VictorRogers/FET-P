using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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

        string daysNum;
        string beginTime;
        string examLength;
        string breakLength;
        string lunchLength;

        string constraintsFile;
        string enrollmentFile;

        //FETP_Controller controller;
        Schedule schedule;

        //Dictionary<string, UserControl> views;

        DataCollection dataCollection1;
        SchedulePresenter scheduleView;

        FullCalendar fullCal;
        SingleDayCalendar miniCal;
        TextSchedule textCal;

        //------------------------------------------------------------------------------------------

        #region FETP_Form events

        /// <summary>
        /// FETP_Form Constructor
        /// Create a new FETP Form with the default Authentication UserControl docked in it
        /// </summary>
        //Author: Amy Brown
        //Date: 3-21-2016
        //Modifications:    Added views Dictionary and delegate events (4-5-2016)
        //                  Moved hookups to dataCollection1's delegate events due to change in startup User Control (2-23-2016)
        //Date(s) Tested:
        //Approved By:
        public FETP_Form()
        {
            InitializeComponent();
            //views = new Dictionary<string, UserControl>();
        }

        /// <summary>
        /// While resizing the SchedulePresenter, keep the splitter in the correct position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Amy Brown
        //Date: 4-7-2016
        //Modifications:    Added check for form minimized
        //Date(s) Tested: 4-7-2016, 4-20-2016
        //Approved By:
        private void FETP_Form_Resize(object sender, EventArgs e)
        {
            if (scheduleView != null && !WindowState.Equals(FormWindowState.Minimized))
            {
                scheduleView.splitContainer1.SplitterDistance = scheduleView.splitContainer1.Width - 221;
            }
        }

        //------------------------------------------------------------------------------------------

        #region Toolstrip Menus

            #region File

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Output: 
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added the reset of form state, size, and style
        //Date(s) Tested:
        //Approved By:
        private void newScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new Size(355, 401);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            panel1.Controls.Clear();

            dataCollection1 = new DataCollection();
            dataCollection1.Dock = DockStyle.Fill;
            dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
            dataCollection1.ClearForm += new DataCollection.ClearClickHandler(ClearAllTextBoxes);

            panel1.Controls.Add(dataCollection1);
        }

        //Author: Cory Feliciano (?)
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
                scheduleView = new SchedulePresenter(schedule);
                scheduleView.Dock = DockStyle.Fill;
                //views.Clear();
                fullCal = new FullCalendar(schedule);
                fullCal.Dock = DockStyle.Fill;
                panel1.Controls.Add(scheduleView);
                scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
                //views.Add("Full", fullCal);
                Size = new Size(681, 492);
                MaximizeBox = true;
                saveAsToolStripMenuItem.Enabled = true;
                viewToolStripMenuItem.Enabled = true;
                oneDayToolStripMenuItem.Enabled = true;
                fullScheduleToolStripMenuItem.Enabled = false;
                textToolStripMenuItem.Enabled = true;
            }
        }

        //Author: Cory Feliciano (?)
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DAT-File | *.dat";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                schedule.SaveSchedule(saveFileDialog.FileName);
            }
        }

        //Author: Cory Feliciano
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //implement export functionality here
        }

            #endregion

            //--------------------------------------------------------------------------------------

            #region View

        /// <summary>
        /// Display the Full Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Output: 
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added dictionary access 
        //Date(s) Tested:
        //Approved By:
        private void fullScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            //if (views.Keys.Contains("Full"))
            //{
            //    fullCal = (FullCalendar)views["Full"];
            //}
            //else
            //{
                fullCal = new FullCalendar(schedule);
                //views.Add("Full", fullCal);
            //}

            fullCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Build and/or Display the Single Day Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Output: 
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added dictionary access
        //Date(s) Tested:
        //Approved By:
        private void oneDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            //if (views.Keys.Contains("Single"))
            //{
            //    miniCal = (SingleDayCalendar)views["Single"];
            //}
            //else
            //{
                //This will need the Schedule data structure as a parameter
                miniCal = new SingleDayCalendar(schedule);
            //    views.Add("Single", miniCal);
            //}

            miniCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(miniCal);
            oneDayToolStripMenuItem.Enabled = false;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Build and/or Display the Text Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Output: 
        //Author: Amy Brown
        //Date: 
        //Date(s) Tested:
        //Approved By:
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            //if (views.Keys.Contains("Text"))
            //{
            //    textCal = (TextSchedule)views["Text"];
            //}
            //else
            //{
                //This will need the Schedule data structure as a parameter
                //Prints entire schedule data structure in agreed format
                textCal = new TextSchedule(schedule);
            //    views.Add("Text", textCal);
            //}

            textCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(textCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = false;
        }

            #endregion

            //--------------------------------------------------------------------------------------

            //This region contains a single function - It will be changed to contain a set of functions
            #region Help

            //Author: Cory Feliciano (?)
            private void helpToolStripMenuItem_Click(object sender, EventArgs e)
            {
                string locationToSavePdf = Path.Combine(Path.GetTempPath(), "HelpManual.pdf");
                File.WriteAllBytes(locationToSavePdf, Properties.Resources.HelpManual);
                Process.Start(locationToSavePdf);
            }

            #endregion

        #endregion

        #endregion

        //------------------------------------------------------------------------------------------

        #region Auth button events
        //Author: Victor Rogers (?)
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        //Author: Victor Rogers (?) and Amy Brown
        //Date:
        //Modifications:    Amy added GUI change implementation in if(isValid){} (4-22-2016)
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
                panel1.Controls.Clear();
                Size = new Size(355, 401);

                dataCollection1 = new DataCollection();
                panel1.Controls.Add(dataCollection1);

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

        //Author: Victor Rogers (?)
        private string GetLoggedInUserName()
        {
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            return currentUser.Name;
        }

        //Author: Victor Rogers (?)
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
        #endregion

        //------------------------------------------------------------------------------------------

        #region DataCollection button events

        /// <summary>
        /// Clear values from all text boxes in Data Collection Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Amy Brown
        //Date: 4-9-2016 
        //Date(s) Tested:
        //Approved By:
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
        //Actions:  Collect filenames in dataCollection1's File name text boxes
        //          Collect values in dataCollection1's data text boxes, check their validity, and convert them to correct data types
        //          Use these values and filenames to generate a schedule object
        //          Use the generated schedule object to generate a SchedulePresenter object and a FullCalendar object
        //          Display generated SchedulePresenter and FullCalendar
        //          Display invalid data messages
        //Output:   SchedulePresenter containing FullCalendar
        //Author:   Amy Brown
        //Date: 3-31-2016
        //Modifications:    Added actual data collection from textbox values (4-9-16)
        //                  Added data value validity checks and type conversions (4-11-16)
        //                  Added creation of schedule object (4-11-16)
        //                  Added views Dictionary Clearout (4-11-16)
        //                  Added invalid data messages and message box (4-11-16)
        //                  Added check for provided Enrollment Data File and error message for missing Enrollment Data File (4-24-2016)
        //Files Accessed:   given Scheule Constraints, given Enrollment Data
        //Date(s) Tested:
        //Approved By:
        public void GenerateFullSchedule(object sender, EventArgs e)
        {
            bool isSchedulePossible = true;
            bool isDaysValid = true;
            bool isBeginValid = true;
            bool isExamValid = true;
            bool isBreakValid = true;
            bool isLunchValid = true;
            bool isEnrollmentValid = true;

            daysNum = dataCollection1.days_textBox.Text;
            beginTime = dataCollection1.startTime_textBox.Text;
            examLength = dataCollection1.examLength_textBox.Text;
            breakLength = dataCollection1.breakLength_textBox.Text;
            lunchLength = dataCollection1.lunchLength_textBox.Text;

            constraintsFile = dataCollection1.scheduleBrowse_textBox.Text;
            enrollmentFile = dataCollection1.enrollmentBrowse_textBox.Text;

            isDaysValid = FETP_Controller.ValidateNumberOfDays(daysNum);
            isBeginValid = FETP_Controller.ValidateExamsStartTime(beginTime);
            isExamValid = FETP_Controller.ValidateExamsLength(examLength);
            isBreakValid = FETP_Controller.ValidateTimeBetweenExams(breakLength);
            isLunchValid = FETP_Controller.ValidateLunchLength(lunchLength);
            isEnrollmentValid = !(enrollmentFile.Equals(string.Empty)); //TODO: implement enrollment file validation function

            isSchedulePossible = (isDaysValid && isBeginValid && isExamValid && isBreakValid && isLunchValid && isEnrollmentValid);
            
            if (isSchedulePossible)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                panel1.Controls.Clear();

                //Create schedule data structure
                schedule = new Schedule(enrollmentFile, daysNum, beginTime, examLength, breakLength, lunchLength);

                //Using schedule data strucutre::
                generateSchedulePresenter(schedule);
                generateFullCalendar(schedule);
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
                    errorMessage += "\nInvalid start time - enter a time from 07:00 to 16:00.";
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
                if (!isEnrollmentValid)
                {
                    errorMessage += "\nPlease provide an Enrollment Data File.";
                }

                MessageBox.Show(errorMessage);
            }
        }

        //Amy
        //SchedulePresenter Constructor builds SplitContainer base presenter - container for different Schedule Views
        private void generateSchedulePresenter(Schedule schedule)
        {
            //views.Clear();

            scheduleView = new SchedulePresenter(schedule);
            scheduleView.Dock = DockStyle.Fill;
            panel1.Controls.Add(scheduleView);

            saveAsToolStripMenuItem.Enabled = true;
            //exportToolStripMenuItem.Enabled = true;   //Uncomment this line when Export is implemented

            viewToolStripMenuItem.Enabled = true;
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        //Amy
        //FullCalendar constructor dynamically builds drag-and-drop button matrix
        private void generateFullCalendar(Schedule schedule)
        {
            fullCal = new FullCalendar(schedule);
            fullCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            //views.Add("Full", fullCal);
            Size = new Size(681, 492);
            MaximizeBox = true;
        }

        #endregion        
    }
}
