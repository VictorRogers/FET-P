﻿using System;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using FETP;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FETP_GUI
{
    public partial class FETP_Form : Form
    {
        string daysNum;
        string beginTime;
        string examLength;
        string breakLength;
        string lunchLength;

        string constraintsFile;
        string enrollmentFile;

        Schedule schedule;

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
        /// Return to the Data Collection screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            saveAsToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// From the File Browser, select a Schedule file to open in the Full Calendar View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                
                FormBorderStyle = FormBorderStyle.Sizable;
                panel1.Controls.Clear();
                scheduleView = new SchedulePresenter(schedule);
                scheduleView.Dock = DockStyle.Fill;
                fullCal = new FullCalendar(schedule);
                fullCal.Dock = DockStyle.Fill;
                panel1.Controls.Add(scheduleView);
                scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
                Size = new Size(681, 492);
                MaximizeBox = true;
                saveAsToolStripMenuItem.Enabled = true;
                exportToolStripMenuItem.Enabled = true;
                viewToolStripMenuItem.Enabled = true;
                oneDayToolStripMenuItem.Enabled = true;
                fullScheduleToolStripMenuItem.Enabled = false;
                textToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// From the File Browser, select a Schedule file to open in the Data Constraints screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openConstraintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DAT-File | *.dat";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = File.OpenRead(openFileDialog.FileName);
                BinaryFormatter formatter = new BinaryFormatter();
                schedule = (Schedule)formatter.Deserialize(stream);
                stream.Close();

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
                saveAsToolStripMenuItem.Enabled = false;

                dataCollection1.scheduleBrowse_textBox.Text = schedule.OriginalConstraintsFilename;
                dataCollection1.enrollmentBrowse_textBox.Text = schedule.OriginalEnrollmentFilename;
                dataCollection1.days_textBox.Text = schedule.OriginalNumberOfDays;
                dataCollection1.startTime_textBox.Text = schedule.OriginalStartTime;
                dataCollection1.examLength_textBox.Text = schedule.OriginalExamLength;
                dataCollection1.breakLength_textBox.Text = schedule.OriginalBreakLength;
                dataCollection1.lunchLength_textBox.Text = schedule.OriginalLunchLength;
            }
        }

        /// <summary>
        /// Set a File Name and Path  to save the current Schedule to a .dat file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Set a File Name and Path to save the current Schedule to a .pdf file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown and Cory Feliciano 5-1-2016
        private void as_PDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF File | *.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                char[] newline = { '\n' };
                TextSchedule textCal = new TextSchedule(schedule);
                string[] lines = textCal.richTextBox1.Text.Split(newline, StringSplitOptions.RemoveEmptyEntries);
                ExportPDFSchedule(saveFileDialog.FileName, lines);
            }
        }
        
        /// <summary>
        /// Write the given text to a .pdf file at the given filepath
        /// </summary>
        /// <param name="path">Location of file</param>
        /// <param name="text">Text to write</param>
        //Amy Brown and Cory Feliciano 5-1-2016
        public void ExportPDFSchedule(string path, string[] text)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            foreach (string s in text)
            {
                char[] tab = { '\t' };
                string[] phrases = s.Split(tab, StringSplitOptions.RemoveEmptyEntries);
                foreach (string p in phrases)
                {
                    doc.Add(new Phrase(p));
                    doc.Add(new Phrase("        "));
                }
                doc.Add(new Phrase("\n"));

            }
            doc.Close();
        }

        /// <summary>
        /// Set a File Name and Path to save the current Schedule to a .txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown and Cory Feliciano 5-1-2016
        private void as_textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File | *.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                char[] newline = { '\n' };
                TextSchedule textCal = new TextSchedule(schedule);
                string[] lines = textCal.richTextBox1.Text.Split(newline, StringSplitOptions.RemoveEmptyEntries);
                schedule.ExportTextSchedule(saveFileDialog.FileName, lines);
            }
        }

        #endregion
        
        //--------------------------------------------------------------------------------------

        #region View

        /// <summary>
        /// Build and Display the Full Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added dictionary access
        //                  removed dictionary access 
        //Date(s) Tested:
        //Approved By:
        private void fullScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            fullCal = new FullCalendar(schedule);
            fullCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Build and Display the Single Day Calendar Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added dictionary access
        //                  Removed dictionary access
        //Date(s) Tested:
        //Approved By:
        private void oneDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            miniCal = new SingleDayCalendar(schedule);
            miniCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(miniCal);
            oneDayToolStripMenuItem.Enabled = false;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Build and Display the Text Schedule View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Amy Brown
        //Date: 
        //Modifications:    Added dictionary access
        //                  Removed dictionary access
        //Date(s) Tested:
        //Approved By:
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleView.splitContainer1.Panel1.Controls.Clear();

            textCal = new TextSchedule(schedule);
            textCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(textCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = false;
        }

        #endregion

        //--------------------------------------------------------------------------------------

        #region Help

        /// <summary>
        /// Open the HelpManual.pdf document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Author: Cory Feliciano
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
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        /// <summary>
        /// If valid creddentials are given, open Data Collection Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Victor Rogers
        public void Login(object sender, EventArgs e)
        {
            bool isValid = false;

            if (auth1.txtUserName.Text.Trim() == GetLoggedInUserName())
            {
                isValid = IsValidCredentials(auth1.txtUserName.Text.Trim(), auth1.txtPwd.Text.Trim(), auth1.txtDomain.Text.Trim());
            }

            if (isValid)
            {
                panel1.Controls.Clear();
                Size = new Size(355, 401);

                dataCollection1 = new DataCollection();
                panel1.Controls.Add(dataCollection1);
                Controls.Add(menuStrip1);

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

        /// <summary>
        /// Get username of currently logged in Windows Account
        /// </summary>
        /// <returns>Username of currently logged in Windows Account</returns>
        //Victor Rogers
        private string GetLoggedInUserName()
        {
            return System.Environment.UserName;
        }

        /// <summary>
        /// Validate given credentials
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Local Network Domain</param>
        /// <returns>isValid -> true if valid credentials, otherwise false</returns>
        //Victor Rogers
        private bool IsValidCredentials(string userName, string password, string domain)
        {
            bool isValid = false;

            if (domain == "")
            {
                domain = Environment.MachineName;
                IntPtr tokenHandler = IntPtr.Zero;
                isValid = LogonUser(userName, domain, password, 2, 0, ref tokenHandler);
            }
            else if (domain == "main.local.una.edu")
            {
                System.DirectoryServices.AccountManagement.PrincipalContext pC = new System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, domain);
                isValid = pC.ValidateCredentials(userName, password);
            }

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
        //Author:   Amy Brown
        //Date: 3-31-2016
        //Modifications:    Added actual data collection from textbox values (4-9-16)
        //                  Added data value validity checks and type conversions (4-11-16)
        //                  Added creation of schedule object (4-11-16)
        //                  Added views Dictionary Clearout (4-11-16)
        //                  Added invalid data messages and message box (4-11-16)
        //                  Added check for provided Enrollment Data File and error message for missing Enrollment Data File (4-24-2016)
        //                  Removed views Dictionary entirely
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
            isEnrollmentValid = !(enrollmentFile.Equals(string.Empty));

            isSchedulePossible = (isDaysValid && isBeginValid && isExamValid && isBreakValid && isLunchValid && isEnrollmentValid);

            if (isSchedulePossible)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                panel1.Controls.Clear();

                //Create schedule data structure
                schedule = new Schedule(enrollmentFile, daysNum, beginTime, examLength, breakLength, lunchLength);

                //Maintain path to original constraints file
                schedule.OriginalConstraintsFilename = dataCollection1.scheduleBrowse_textBox.Text;

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

        /// <summary>
        /// Build and Display SplitContainer base presenter - container for different Schedule Views
        /// </summary>
        /// <param name="schedule"></param>
        //Amy Brown
        private void generateSchedulePresenter(Schedule schedule)
        {
            scheduleView = new SchedulePresenter(schedule);
            scheduleView.Dock = DockStyle.Fill;
            panel1.Controls.Add(scheduleView);

            saveAsToolStripMenuItem.Enabled = true;
            exportToolStripMenuItem.Enabled = true;

            viewToolStripMenuItem.Enabled = true;
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Dynamically Build and Display drag-and-drop button matrix
        /// </summary>
        /// <param name="schedule"></param>
        //Amy Brown
        private void generateFullCalendar(Schedule schedule)
        {
            fullCal = new FullCalendar(schedule);
            fullCal.Dock = DockStyle.Fill;

            scheduleView.splitContainer1.Panel1.Controls.Add(fullCal);
            Size = new Size(681, 492);
            MaximizeBox = true;
        }

        #endregion
    }
}
