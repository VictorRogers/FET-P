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
        SchedulePresenter schedule;
        FullCalendar fullCal;
        SingleDayCalendar miniCal;
        TextSchedule textCal;

        public FETP_Form()
        {
            InitializeComponent();
            dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
        }

        public void GenerateFullSchedule(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            //SchedulePresenter Constructor takes care of getting actual schedule data
            schedule = new SchedulePresenter();
            schedule.Dock = DockStyle.Fill;

            //FullCalendar constructor dynamically builds drag-and-drop button matrix
            fullCal = new FullCalendar();
            fullCal.Dock = DockStyle.Fill;

            panel1.Controls.Add(schedule);
            schedule.splitContainer1.Panel1.Controls.Add(fullCal);
            Size = new Size(681, 492);
            MaximizeBox = true;
            viewToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            schedule.splitContainer1.Panel1.Controls.Clear();

            textCal = new TextSchedule();
            textCal.Dock = DockStyle.Fill;

            schedule.splitContainer1.Panel1.Controls.Add(textCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = false;
        }

        private void fullScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            schedule.splitContainer1.Panel1.Controls.Clear();

            fullCal = new FullCalendar();
            fullCal.Dock = DockStyle.Fill;

            schedule.splitContainer1.Panel1.Controls.Add(fullCal);
            oneDayToolStripMenuItem.Enabled = true;
            fullScheduleToolStripMenuItem.Enabled = false;
            textToolStripMenuItem.Enabled = true;
        }

        private void oneDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            schedule.splitContainer1.Panel1.Controls.Clear();

            miniCal = new SingleDayCalendar();
            miniCal.Dock = DockStyle.Fill;

            schedule.splitContainer1.Panel1.Controls.Add(miniCal);
            oneDayToolStripMenuItem.Enabled = false;
            fullScheduleToolStripMenuItem.Enabled = true;
            textToolStripMenuItem.Enabled = true;
        }
    }
}
