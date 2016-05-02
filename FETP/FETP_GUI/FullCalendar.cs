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
using CalendarExtension;

namespace FETP_GUI
{
    public partial class FullCalendar : UserControl
    {
        private bool selectOrSwitch = true;
        private Button tempForSwap;

        private int NUMBER_OF_DAYS;
        private int NUMBER_OF_EXAMS_PER_DAY;

        private GroupBox[] Days;
        private Panel[] DayPanels;
        private Button[][] Exams;
        private Label[][] startTimes;

        Schedule _schedule;

        /// <summary>
        /// 
        /// </summary>
        public FullCalendar()
        {
            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY);
        }

        /// <summary>
        /// Get generated schedule from FETP_Form
        /// </summary>
        /// <param name="schedule">Schedule object to display</param>
        public FullCalendar(Schedule schedule)
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
        //4-14-16: Added labels for start times
        //4-20-16: Added labels on exam buttons
        private void InitializeComponent(int numOfDays, int numOfExamsPerDay)
        {
            #region Initialize new GUI objects
            components = new Container();
            Days = new GroupBox[numOfDays];
            DayPanels = new Panel[numOfDays];

            int day = 0;
            for (; day < numOfDays; day++)
            {
                Days[day] = new GroupBox();
                DayPanels[day] = new Panel();
                Exams[day] = new Button[numOfExamsPerDay];
                startTimes[day] = new Label[numOfExamsPerDay];
                for (int n = 0; n < numOfExamsPerDay; n++)
                {
                    Exams[day][n] = new Button();
                    startTimes[day][n] = new Label();
                }
            }
            toolTip1 = new ToolTip(components);
            #endregion

            #region suspend layout
            foreach (GroupBox d in Days)
            {
                d.SuspendLayout();
            }
            SuspendLayout();
            #endregion

            #region set Dynamic GUI object properties
            //
            // Days
            //
            day = 0;
            foreach (GroupBox gb in Days)
            {
                gb.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom);
                gb.Controls.Add(DayPanels[day]);
                gb.BackColor = Color.FromArgb(45, 12, 73);
                gb.Font = new Font("Microsoft Sans Serif", 16.0F, FontStyle.Bold, GraphicsUnit.Point, 0);
                gb.ForeColor = Color.FromArgb(219, 159, 17);
                gb.Location = new Point((201 + 15) * day, 0);
                gb.Name = "Day " + (day + 1).ToString();
                gb.Size = new Size(200, 450);
                gb.Text = gb.Name;

                day++;
            }

            //
            //DayPanels
            //
            day = 0;
            foreach (Panel p in DayPanels)
            {
                p.AllowDrop = true;

                p.AutoScroll = true;
                p.BackColor = Color.Transparent;
                for (int n = numOfExamsPerDay - 1; n >= 0; n--)
                {
                    p.Controls.Add(Exams[day][n]);
                    p.Controls.Add(startTimes[day][n]);
                }
                p.Dock = DockStyle.Fill;

                day++;
            }

            //
            // Exams
            //
            day = 0;
            while (day < numOfDays)
            {
                int block = 0;
                foreach (Button b in Exams[day])
                {
                    b.AllowDrop = true;

                    b.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    b.BackColor = Color.FromArgb(219, 159, 17);
                    b.FlatAppearance.BorderColor = Color.FromArgb(142, 105, 18);
                    b.FlatAppearance.BorderSize = 2;
                    b.FlatStyle = FlatStyle.Flat;
                    b.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    b.ForeColor = Color.FromArgb(70, 22, 107);
                    b.Location = new Point(6, (32 + (100) * block));
                    b.Name = "Exam Time " + (block + 1).ToString();
                    b.Size = new Size(185, 68);
                    //b.Text = b.Name;
                    b.UseVisualStyleBackColor = false;

                    b.Click += button_Click;

                    block++;
                }
                day++;
            }
            this.labelAllScheduledBlocks(_schedule, ref Exams);
            tooltipAllScheduledBlocks(_schedule, ref Exams);

            //
            // _startTimes
            //
            day = 0;
            while (day < numOfDays)
            {
                int y = 0;
                foreach (Label l in startTimes[day])
                {
                    l.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    l.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    l.ForeColor = Color.FromArgb(142, 105, 18);
                    l.Location = new Point(6, (16 + ((100) * y)));
                    l.Size = new Size(185, 68);
                    l.Text = _schedule.StartTimesOfExams[y].ToString();
                    y++;
                }
                day++;
            }
            #endregion

            //
            //FullCalendar
            //
            AutoScroll = true;
            for (day = numOfDays - 1; day >= 0; day--)
            {
                Controls.Add(Days[day]);
            }
            Dock = DockStyle.Fill;
            Size = new Size(433, 466);

            #region Resume layout
            foreach (GroupBox d in Days)
            {
                d.ResumeLayout(false);
            }
            ResumeLayout(false);
            #endregion

            PerformLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_schedule"></param>
        /// <param name="Exams"></param>
        public void tooltipAllScheduledBlocks(Schedule _schedule, ref Button[][] Exams)
        {
            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;

            int day = 0, nulls = 0;
            for (int a = 0; a < _schedule.Blocks.Count(); a++)
            {
                int block = (a - nulls) % _schedule.NumberOfTimeSlotsAvailablePerDay;
                if (_schedule.Blocks[a] != null)
                {
                    Exams[day][block].Tag += _schedule.Blocks[a].ClassesInBlock.Count.ToString() + " class(es) \t" + _schedule.Blocks[a].Enrollment.ToString() + " total students\n";

                    foreach (Class c in _schedule.Blocks[a].ClassesInBlock)
                    {
                        foreach (DayOfWeek d in c.DaysMeet)
                        {
                            switch (d)
                            {
                                case DayOfWeek.Monday:
                                    Exams[day][block].Tag += "M";
                                    break;
                                case DayOfWeek.Tuesday:
                                    Exams[day][block].Tag += "T";
                                    break;
                                case DayOfWeek.Wednesday:
                                    Exams[day][block].Tag += "W";
                                    break;
                                case DayOfWeek.Thursday:
                                    Exams[day][block].Tag += "R";
                                    break;
                                case DayOfWeek.Friday:
                                    Exams[day][block].Tag += "F";
                                    break;
                            }
                        }

                        Exams[day][block].Tag += "\t" + c.StartTime.ToString();
                        Exams[day][block].Tag += "-" + c.EndTime.ToString();
                        Exams[day][block].Tag += "\t(" + c.Enrollment.ToString() + " students)";
                        Exams[day][block].Tag += "\n";
                    }
                    toolTip1.SetToolTip(Exams[day][block], Exams[day][block].Tag.ToString());

                    if (block.Equals(totalPerDay)) { day++; }
                }
                else { nulls++; }
            }
        }

        //------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            if (selectOrSwitch)
            {
                //select sender button
                tempForSwap = (Button)sender;
            }

            //I am aware that this is a mess
            else
            {
                bool swappedBlocks = false;
                //swap selected and sender button
                int x1 = 0;
                int y1 = 0;
                for(x1 = 0; x1< Exams.Length; x1++)
                {
                    for (y1 = 0; y1 < Exams[x1].Length; y1++)
                    {
                        if (Exams[x1][y1].Equals(tempForSwap))
                        {
                            SwapButtons(x1, y1, Exams[x1][y1], (Button)sender);
                            SwapBlocks(x1, y1, (Button)sender);
                            swappedBlocks = true;
                            break;
                        }
                    }
                    if (swappedBlocks)
                    {
                        break;
                    }
                }

                //swap button's position within Exams[][]
                bool swappedButtons = false;
                for (int x2 = 0; x2 < Exams.Length; x2++)
                {
                    for (int y2 = 0; y2 < Exams[x2].Length; y2++)
                    {
                        if (Exams[x2][y2].Equals((Button)sender))
                        {
                            Button temp = Exams[x2][y2];
                            Exams[x2][y2] = Exams[x1][y1];
                            Exams[x1][y1] = temp;
                            swappedButtons = true;
                            break;
                        }
                    }
                    if (swappedButtons)
                    {
                        break;
                    }
                }
            }

            selectOrSwitch = !selectOrSwitch;
        }

        /// <summary>
        /// Swap Buttons display parent containers, locations, and sizes
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void SwapButtons(int x1, int y1, Button source, Button destination)
        {
            Panel p1 = (Panel)source.Parent;
            Panel p2 = (Panel)destination.Parent;

            Point l1 = source.Location;
            Point l2 = destination.Location;

            Size s1 = source.Size;
            Size s2 = destination.Size;

            p1.Controls.Remove(source);
            p2.Controls.Remove(destination);

            p1.Controls.Add(destination);
            p2.Controls.Add(source);

            source.Parent = p2;
            destination.Parent = p1;

            source.Location = l2;
            destination.Location = l1;

            source.Size = s2;
            destination.Size = s1;

            source.BringToFront();
            destination.BringToFront();

            p1.Refresh();
            p2.Refresh();
        }

        /// <summary>
        /// Find and swap positions in _schedule.Blocks[] of Blocks indicated by Buttons
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="destination"></param>
        private void SwapBlocks(int i, int j, Button destination)
        {
            int sourceBlock = i * _schedule.NumberOfTimeSlotsAvailablePerDay + j;
            int destinationBlock = 0;
            bool foundDestination = false;

            for (int k = 0; k < Exams.Length; k++)
            {
                for (int l = 0; l < Exams[k].Length; l++)
                {
                    if (Exams[k][l].Equals(destination))
                    {
                        destinationBlock = k * _schedule.NumberOfTimeSlotsAvailablePerDay + l;
                        foundDestination = true;
                        break;
                    }
                }
                if (foundDestination)
                {
                    break;
                }
            }

            _schedule.SwitchBlocks(sourceBlock, destinationBlock);
        }        
    }
}