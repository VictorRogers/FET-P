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
        public FETP_Form()
        {
            InitializeComponent();
            dataCollection1.GenerateSchedule += new DataCollection.GenerateClickHandler(GenerateFullSchedule);
        }

        public void GenerateFullSchedule(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            //SchedulePresenter Constructor takes care of getting actual schedule data
            SchedulePresenter schedule = new SchedulePresenter();
            schedule.Dock = DockStyle.Fill;

            //FullCalendar constructor dynamically builds drag-and-drop button matrix
            FullCalendar fullCal = new FullCalendar();
            fullCal.Dock = DockStyle.Fill;

            panel1.Controls.Add(schedule);
            schedule.splitContainer1.Panel1.Controls.Add(fullCal);
            this.Size = new Size(681, 492);
            this.MaximizeBox = true;
        }
    }
}
