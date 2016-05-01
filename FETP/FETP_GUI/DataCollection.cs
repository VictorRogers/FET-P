using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

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

        //Amy
        private void generate_Click(object sender, EventArgs e)
        {
            if (GenerateSchedule != null)
            {
                GenerateSchedule(this, e);
            }
        }

        //Amy
        private void clear_Click(object sender, EventArgs e)
        {
            if (ClearForm != null)
            {
                ClearForm(this, e);
            }
        }

        private void enrollmentBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV-File | *.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                enrollmentBrowse_textBox.Text = openFileDialog.FileName;
            }
        }

        //Cory? and Amy
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

        private void scheduleBrowse_textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
