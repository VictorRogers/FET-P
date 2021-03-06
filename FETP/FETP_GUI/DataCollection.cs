﻿using System;
using System.Windows.Forms;
using System.IO;

namespace FETP_GUI
{
    public partial class DataCollection : UserControl
    {
        public delegate void GenerateClickHandler(object sender, EventArgs e);
        public event GenerateClickHandler GenerateSchedule;

        public delegate void ClearClickHandler(object sender, EventArgs e);
        public event ClearClickHandler ClearForm;

        public DataCollection()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegate Event Handler for the Generate Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown
        private void generate_Click(object sender, EventArgs e)
        {
            if (GenerateSchedule != null)
            {
                GenerateSchedule(this, e);
            }
        }

        /// <summary>
        /// Delegate Event Handler for the Clear Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown
        private void clear_Click(object sender, EventArgs e)
        {
            if (ClearForm != null)
            {
                ClearForm(this, e);
            }
        }

        /// <summary>
        /// Open File Dialog Window to find Enrollment Data File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enrollmentBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV-File | *.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                enrollmentBrowse_textBox.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Open File Dialog Window to find Schedule Constraints Data File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Cory Feliciano and Amy Brown
        private void scheduleBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT-File | *.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                scheduleBrowse_textBox.Text = openFileDialog.FileName;


                FileStream inFile = File.OpenRead(@openFileDialog.FileName);
                var reader = new StreamReader(inFile);

                days_textBox.Text = int.Parse(reader.ReadLine()).ToString();
                startTime_textBox.Text = reader.ReadLine();
                examLength_textBox.Text = int.Parse(reader.ReadLine()).ToString();
                breakLength_textBox.Text = int.Parse(reader.ReadLine()).ToString();
                lunchLength_textBox.Text = int.Parse(reader.ReadLine()).ToString();
            }
        }        
    }
}
