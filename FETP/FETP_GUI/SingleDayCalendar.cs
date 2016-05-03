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
    public partial class SingleDayCalendar : UserControl
    {
        private int NUMBER_OF_DAYS;
        private int NUMBER_OF_EXAMS_PER_DAY;

        private int WHAT_DAY = 0;

        private bool selectOrSwitch = true;
        private Button tempForSwap;

        private GroupBox[] Days;
        private Panel[] DayPanels;
        private Button[][] Exams;
        private Label[][] startTimes;

        Schedule _schedule;
        
        /// <summary>
        /// Dynamic constructor for SingleDayCalendar using data from given Schedule object
        /// </summary>
        /// <param name="schedule">Schedule object containing data used to build calendar view</param>
        //Amy Brown
        public SingleDayCalendar(Schedule schedule)
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
        //Amy Brown
        //4-14-16: Added labels for start times
        private void InitializeComponent(int numOfDays, int numOfExamsPerDay)
        {
            #region Initialize new GUI objects
            components = new Container();
            previous = new Button();
            next = new Button();
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

            #region set GUI object properties
            #region Dynamic GUI Objects
            //
            // Days
            //
            day = 0;
            foreach (GroupBox gb in Days)
            {
                gb.AutoSize = true;
                gb.Controls.Add(DayPanels[day]);
                gb.Dock = DockStyle.Fill;
                gb.Enabled = true;
                gb.BackColor = Color.FromArgb(70, 22, 107);
                gb.Font = new Font("Microsoft Sans Serif", 16.0F, FontStyle.Bold, GraphicsUnit.Point, 0);
                gb.ForeColor = Color.FromArgb(219, 159, 17);
                gb.Name = "Day " + (day + 1).ToString();
                gb.Size = new Size(200, ((68 + 15) * (numOfExamsPerDay) + 15));
                gb.Text = gb.Name;

                day++;
            }

            //
            //DayPanels
            //
            day = 0;
            foreach (Panel p in DayPanels)
            {
                p.AutoScroll = true;
                p.AutoSize = true;
                p.BackColor = Color.Transparent;
                for (int block = numOfExamsPerDay - 1; block >= 0; block--)
                {
                    p.Controls.Add(Exams[day][block]);
                    p.Controls.Add(startTimes[day][block]);
                }
                p.Dock = DockStyle.Fill;
                p.Size = new Size(200, (68 + 15) * (numOfExamsPerDay + 1));

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
                    b.UseVisualStyleBackColor = false;

                    b.Click += button_Click;

                    block++;
                }
                day++;
            }
            this.labelAllScheduledBlocks(_schedule, ref Exams);
            tagAllScheduledBlocks(_schedule, ref Exams);

            //
            // startTimes
            //
            day = 0;
            while (day < numOfDays)
            {
                int block = 0;
                foreach (Label l in startTimes[day])
                {
                    l.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    l.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    l.ForeColor = Color.FromArgb(142, 105, 18);
                    l.Location = new Point(6, (16 + ((100) * block)));
                    l.Size = new Size(185, 68);
                    l.Text = _schedule.StartTimesOfExams[block].ToString();
                    block++;
                }
                day++;
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
            //tootlTip1
            //
            toolTip1.ShowAlways = true;
            toolTip1.Active = true;
            //
            //SingleDayCalendar
            //
            Controls.Add(Days[WHAT_DAY]);
            ActiveControl = Days[WHAT_DAY];

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

        /// <summary>
        /// Set Tag text for each Button in the SingleDayCalendar
        /// </summary>
        /// <param name="_schedule">Schedule object containing data to set in Tag text</param>
        /// <param name="Exams">Set of Buttons in SingleDayCalendar</param>
        //Amy Brown
        public void tagAllScheduledBlocks(Schedule _schedule, ref Button[][] Exams)
        {
            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;

            int day = 0, nulls = 0;
            for (int a = 0; a < _schedule.Blocks.Count(); a++)
            {
                int block = (a - nulls) % _schedule.NumberOfTimeSlotsAvailablePerDay;
                if (_schedule.Blocks[a] != null)
                {
                    Exams[day][block].Tag += _schedule.Blocks[a].ClassesInBlock.Count.ToString() + " class(es)\t" + _schedule.Blocks[a].Enrollment.ToString() + " total students\n";

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

        /// <summary>
        /// Go to previous day in the schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown
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
                ActiveControl = Days[WHAT_DAY];

                Controls.Add(next);
                Controls.Add(previous);
            }
        }

        /// <summary>
        /// Go to next day in the schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Event Handler for clicking any exam time Button in the SingleDayCalendar view
        /// 1st click selects one Button
        /// 2nd click selects second Button and switches both the Buttons and the Block objects within the Schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Amy Brown
        private void button_Click(object sender, EventArgs e)
        {
            if (selectOrSwitch)
            {
                //select sender button
                tempForSwap = (Button)sender;

                //Highlight selected sender Button
                Button b = (Button)sender;
                b.Select();
            }

            //I am aware that this is a mess
            else
            {
                bool swappedBlocks = false;
                //swap selected and sender button
                int x1 = 0;
                int y1 = 0;
                for (x1 = 0; x1 < Exams.Length; x1++)
                {
                    for (y1 = 0; y1 < Exams[x1].Length; y1++)
                    {
                        if (Exams[x1][y1].Equals(tempForSwap))
                        {
                            SwapButtons(Exams[x1][y1], (Button)sender);
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

                //swap buttons position within Exams[][]
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
        /// <param name="source">1st selected Button</param>
        /// <param name="destination">2nd selected Button</param>
        //Amy Brown
        private void SwapButtons(Button source, Button destination)
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
        /// <param name="i">x-coordinate of 1st selected Button within Exams[][]</param>
        /// <param name="j">y-coordinate of 1st selected Button within Exams[][]</param>
        /// <param name="destination">2nd selected Button</param>
        //Amy Brown
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
