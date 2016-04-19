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
    public partial class SingleDayCalendar : UserControl
    {
        //private static int NUMBER_OF_DAYS = 4;
        //private static int NUMBER_OF_EXAMS_PER_DAY = 10;

        private int NUMBER_OF_DAYS;
        private int NUMBER_OF_EXAMS_PER_DAY;

        private int WHAT_DAY = 0;

        private GroupBox[] Days;
        private Panel[] DayPanels;
        private Button[][] Exams;
        private Label[][] startTimes;// = new Label[NUMBER_OF_DAYS][];

        Schedule _schedule;

        public SingleDayCalendar()
        {
            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY);
        }

        //Add Schedule object parameter - get generated schedule from FETP_Form
        public SingleDayCalendar(Schedule schedule, int examLength, int breakLength, int lunchLength)
        {
            _schedule = schedule;
            NUMBER_OF_DAYS = schedule.NumberOfDays;
            NUMBER_OF_EXAMS_PER_DAY = schedule.NumberOfTimeSlotsAvailablePerDay;
            Exams = new Button[NUMBER_OF_DAYS][];
            startTimes = new Label[NUMBER_OF_DAYS][];

            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY);
        }

        /// <summary>
        /// Dynamic Initializer using static ints and no event handlers
        /// </summary>
        /// <param name="numOfDays">Number of days in the schedule</param>
        /// <param name="numOfExamsPerDay">Number of exam time slots per day</param>
        /// <param name="NUMBER_OF_EXAMS">Total number of exam time slots in the schedule</param>
        /// //4-14-16: Added labels for start times
        private void InitializeComponent(int numOfDays, int numOfExamsPerDay)
        {
            #region Initialize new GUI objects
            previous = new Button();
            next = new Button();
            Days = new GroupBox[numOfDays];
            DayPanels = new Panel[numOfDays];

            int i = 0;
            for (; i < numOfDays; i++)
            {
                Days[i] = new GroupBox();
                DayPanels[i] = new Panel();
                Exams[i] = new Button[numOfExamsPerDay];
                startTimes[i] = new Label[numOfExamsPerDay];
                for (int n = 0; n < numOfExamsPerDay; n++)
                {
                    Exams[i][n] = new Button();
                    startTimes[i][n] = new Label();
                }
            }
            #endregion

            #region suspend layout
            foreach (GroupBox d in Days)
            {
                d.SuspendLayout();
            }
            SuspendLayout();
            #endregion

            #region set GUI object properties
            #region Dynamic GUI Objects
            //
            // Days
            //
            i = 0;
            foreach (GroupBox gb in Days)
            {
                gb.AutoSize = true;
                gb.Controls.Add(DayPanels[i]);
                gb.Dock = DockStyle.Fill;
                gb.BackColor = Color.FromArgb(70, 22, 107);
                gb.Font = new Font("Microsoft Sans Serif", 16.0F, FontStyle.Bold, GraphicsUnit.Point, 0);
                gb.ForeColor = Color.FromArgb(219, 159, 17);
                gb.Name = "Day " + (i + 1).ToString();
                gb.Size = new Size(200, ((68 + 15) * (numOfExamsPerDay) + 15));
                gb.Text = gb.Name;

                i++;
            }

            //
            //DayPanels
            //
            i = 0;
            foreach (Panel p in DayPanels)
            {
                p.AutoScroll = true;
                p.AutoSize = true;
                p.BackColor = Color.Transparent;
                for (int n = numOfExamsPerDay - 1; n >= 0; n--)
                {
                    p.Controls.Add(Exams[i][n]);
                    p.Controls.Add(startTimes[i][n]);
                }
                p.Dock = DockStyle.Fill;
                p.Size = new Size(200, (68 + 15) * (numOfExamsPerDay + 1));

                i++;
            }

            //
            // Exams
            //
            int j = 0, k = 1;
            while (j < numOfDays)
            {
                int y = 0;
                foreach (Button b in Exams[j])
                {
                    b.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    b.BackColor = Color.FromArgb(219, 159, 17);
                    b.FlatAppearance.BorderColor = Color.FromArgb(142, 105, 18);
                    b.FlatAppearance.BorderSize = 2;
                    b.FlatStyle = FlatStyle.Flat;
                    b.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    b.ForeColor = Color.FromArgb(70, 22, 107);
                    b.Location = new Point(6, (32 + (100) * y));
                    b.Name = "Exam Time " + (k).ToString();
                    b.Size = new Size(185, 68);
                    b.Text = b.Name;
                    b.UseVisualStyleBackColor = false;
                    y++; k++;
                }
                j++;
            }

            //
            // startTimes
            //
            j = 0;
            while (j < numOfDays)
            {
                int y = 0;
                foreach (Label l in startTimes[j])
                {
                    l.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    l.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    l.ForeColor = Color.FromArgb(142, 105, 18);
                    l.Location = new Point(6, (16 + ((100) * y)));
                    l.Size = new Size(185, 68);
                    l.Text = _schedule.StartTimesOfExams[y].ToString();
                    y++;
                }
                j++;
            }
            #endregion

            #region These things stay the same
            // 
            // next
            // 
            next.BackColor = Color.FromArgb(45, 12, 73);
            next.Dock = DockStyle.Right;
            next.Enabled = (Days.Length.Equals(1)) ? false : true;
            next.FlatStyle = FlatStyle.Flat;
            next.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            next.ForeColor = Color.FromArgb(219, 159, 17);
            next.Location = new Point(361, 0);
            next.Name = "next";
            next.Size = new Size(75, 389);
            next.TabIndex = 5;
            next.Text = "--->    Next Day";
            next.UseVisualStyleBackColor = false;
            next.Click += new EventHandler(next_Click);
            // 
            // previous
            // 
            previous.BackColor = Color.FromArgb(45, 12, 73);
            previous.Dock = DockStyle.Left;
            previous.Enabled = false;
            previous.FlatStyle = FlatStyle.Flat;
            previous.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            previous.ForeColor = Color.FromArgb(219, 159, 17);
            previous.Location = new Point(0, 0);
            previous.Name = "previous";
            previous.Size = new Size(75, 389);
            previous.TabIndex = 4;
            previous.Text = "<---   Prev. Day";
            previous.UseVisualStyleBackColor = false;
            previous.Click += new EventHandler(previous_Click);
            //
            //SingleDayCalendar
            //
            //AutoScroll = true;
            Controls.Add(Days[WHAT_DAY]);
            Controls.Add(next);
            Controls.Add(previous);
            AutoScroll = true;
            Dock = DockStyle.Fill;
            #endregion
            #endregion

            #region Resume layout
            foreach (GroupBox d in Days)
            {
                d.ResumeLayout(false);
            }
            ResumeLayout(false);
            #endregion

            PerformLayout();
        }

        private void previous_Click(object sender, EventArgs e)
        {
            if (!WHAT_DAY.Equals(0))
            {
                Controls.Clear();

                WHAT_DAY--;
                if (WHAT_DAY.Equals(0))
                {
                    previous.Enabled = false;
                }
                else
                {
                    previous.Enabled = true;
                }

                if (!WHAT_DAY.Equals(NUMBER_OF_DAYS - 1))
                {
                    next.Enabled = true;
                }

                Controls.Add(Days[WHAT_DAY]);
                Controls.Add(next);
                Controls.Add(previous);
            }
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (!WHAT_DAY.Equals(NUMBER_OF_DAYS - 1))
            {
                Controls.Clear();

                WHAT_DAY++;
                if (WHAT_DAY.Equals(NUMBER_OF_DAYS - 1))
                {
                    next.Enabled = false;
                }
                else
                {
                    next.Enabled = true;
                }

                if (!WHAT_DAY.Equals(0))
                {
                    previous.Enabled = true;
                }

                Controls.Add(Days[WHAT_DAY]);
                Controls.Add(next);
                Controls.Add(previous);
            }
        }
    }
}
