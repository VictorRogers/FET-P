
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
    public partial class SchedulePresenter : UserControl
    {
        int NUMBER_OF_BLOCKS;
        int NULL_BLOCKS;

        private GroupBox classGroups;
        private Button[] Blocks;// = new Button[NUMBER_OF_EXAMS];
                                //All the Button objects are going to need to be replaced with an extended Button object
                                //Extended Button object has members Block and Label
                                //Label is a drag-and-drop GUI object
                                //Block is attached schedule data

        //Maybe I don't actually care about the extended Button class.
        //I can probablyjust drag and drop the actual buttons themselves and just use button.text for the label

        ///<summary>
        ///Generic, unused SchedulePresenter constructor (this was made for design phase)
        ///</summary>
        public SchedulePresenter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dynamic SchedulePresnter constructor determines NUMBER_OF_EXAMS and builds the view presenter
        /// </summary>
        /// <param name="daysNum">Number of Days in the schedule, determined by scheduler algorithm</param>
        /// <param name="examsPerDay">Number of Exams per Day, determined by scheduler algorithm</param>
        public SchedulePresenter(Schedule schedule)
        {
            NUMBER_OF_BLOCKS = schedule.Blocks.Count() + schedule.LeftoverBlocks.Count();

            NULL_BLOCKS = 0;

            foreach (Block b in schedule.Blocks)
            {
                if (b == null)
                {
                    NULL_BLOCKS++;
                }
            }

            Blocks = new Button[NUMBER_OF_BLOCKS - NULL_BLOCKS];

            InitializeComponent(schedule);
        }

        /// <summary>
        /// Builds the SchedulePresenter object based on total NUMBER_OF_EXAMS
        /// </summary>
        /// <param name="numOfExams">Total Number of Exams in the schedule</param>
        private void InitializeComponent(Schedule schedule)
        {
            int numOfScheduled = schedule.Blocks.Count() - NULL_BLOCKS;
            int numofUnscheduled = schedule.LeftoverBlocks.Count();

            #region Initialize new GUI objects
            components = new Container();
            splitContainer1 = new SplitContainer();

            int i = 0;
            for (; i < NUMBER_OF_BLOCKS - NULL_BLOCKS; i++)
            {
                Blocks[i] = new Button();
            }

            classGroups = new GroupBox();
            panel1 = new Panel();
            toolTip1 = new ToolTip(components);
            #endregion

            ((ISupportInitialize)(splitContainer1)).BeginInit();

            #region suspend layout
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            classGroups.SuspendLayout();
            SuspendLayout();
            #endregion

            #region set GUI object properties
            #region These things stay the same
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            //
            splitContainer1.Panel1.BackColor = Color.FromArgb(70, 22, 107);
            // 
            // splitContainer1.Panel2
            //
            splitContainer1.Panel2.BackColor = Color.FromArgb(45, 12, 73);
            splitContainer1.Panel2.Controls.Add(classGroups);
            splitContainer1.Size = new Size(648, 466);
            splitContainer1.SplitterDistance = 427;
            splitContainer1.TabIndex = 3;
            //
            // classGroups
            //
            classGroups.BackColor = Color.FromArgb(70, 22, 107);
            classGroups.Controls.Add(panel1);
            classGroups.Dock = DockStyle.Fill;
            classGroups.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            classGroups.ForeColor = Color.FromArgb(219, 159, 17);
            classGroups.Location = new Point(0, 0);
            classGroups.Name = "groupBox1";
            classGroups.Size = new Size(222, 858);
            classGroups.TabIndex = 0;
            classGroups.TabStop = false;
            classGroups.Text = "Class Groups";
            #endregion

            #region Dynamic GUI objects            
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.Transparent;
            i = NUMBER_OF_BLOCKS - NULL_BLOCKS - 1;
            for (; i >= 0; i--)
            {
                panel1.Controls.Add(Blocks[i]);
            }
            panel1.Dock = DockStyle.Fill;

            //
            // Blocks
            //
            i = 0;
            for (; i < numOfScheduled; i++)
            {
                //Blocks[i].Dock = DockStyle.Top;
                Blocks[i].BackColor = Color.FromArgb(45, 12, 73);
                Blocks[i].Enabled = true;
                Blocks[i].FlatAppearance.BorderSize = 2;
                Blocks[i].FlatStyle = FlatStyle.Flat;
                Blocks[i].Location = new Point(6, (15 + ((15 + 68) * i)));
                Blocks[i].Name = "Group #" + (i + 1).ToString();
                Blocks[i].Size = new Size(180, 68);
                //Blocks[i].Text = Blocks[i].Name;
                Blocks[i].UseVisualStyleBackColor = false;
                //i++;
            }
            i = numOfScheduled;
            for (; i < NUMBER_OF_BLOCKS - NULL_BLOCKS; i++)
            {
                //Blocks[i].Dock = DockStyle.Top;
                Blocks[i].BackColor = Color.FromArgb(142, 105, 18);
                Blocks[i].FlatAppearance.BorderColor = Color.FromArgb(219, 159, 17); 
                Blocks[i].Enabled = true;
                Blocks[i].FlatAppearance.BorderSize = 2;
                Blocks[i].FlatStyle = FlatStyle.Flat;
                Blocks[i].ForeColor = Color.FromArgb(45, 12, 73);
                Blocks[i].Location = new Point(6, (15 + ((15 + 68) * i)));
                Blocks[i].Name = "Group #" + (i + 1).ToString();
                Blocks[i].Size = new Size(180, 68);
                //Blocks[i].Text = Blocks[i].Name;
                Blocks[i].UseVisualStyleBackColor = false;
                //i++;
            }
            this.labelAllListedBlocks(schedule, ref Blocks);
            tooltipAllListedBlocks(schedule, ref Blocks);

            #endregion

            // 
            // SchedulePresenter
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "FullCalendar";
            Size = new Size(648, 466);
            #endregion

            #region resume layout
            splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            classGroups.ResumeLayout(false);
            ResumeLayout(false);
            #endregion
        }


        public void tooltipAllListedBlocks(Schedule _schedule, ref Button[] ButtonBlocks)
        {
            
            int nulls = 0;
            foreach(Block b in _schedule.Blocks)
            {
                if (b == null)
                {
                    nulls++;
                }
            }

            Block[] nonNullBlocks = new Block[_schedule.Blocks.Count() - nulls];
            //for(int j=0; (j < (_schedule.Blocks.Count()) && _schedule.Blocks[j] != null); j++)
            //{
            //    nonNullBlocks[j] = _schedule.Blocks[j];
            //}

            for (int i=0, j = 0; i < _schedule.Blocks.Count(); i++, j++)
            {
                if(_schedule.Blocks[i] != null)
                {
                    nonNullBlocks[j] = _schedule.Blocks[i];
                }
                else
                {
                    j--;
                    continue;
                }
            }

            Block[] sortedBlocks = nonNullBlocks.OrderByDescending(b => b.Enrollment).ToArray();

            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;

            nulls = 0;
            int block = 0;
            for (; block < sortedBlocks.Length; block++)
            {
                int button = (block - nulls);
                if (sortedBlocks[block] != null)
                {
                    ButtonBlocks[button].Tag = sortedBlocks[block].ClassesInBlock.Count.ToString() + " class(es) \t" + sortedBlocks[block].Enrollment.ToString() + " total students\n";
                    
                    foreach(Class c in sortedBlocks[block].ClassesInBlock)
                    {
                        foreach (DayOfWeek d in c.DaysMeet)
                        {
                            switch (d)
                            {
                                case DayOfWeek.Monday:
                                    ButtonBlocks[button].Tag += "M";
                                    break;
                                case DayOfWeek.Tuesday:
                                    ButtonBlocks[button].Tag += "T";
                                    break;
                                case DayOfWeek.Wednesday:
                                    ButtonBlocks[button].Tag += "W";
                                    break;
                                case DayOfWeek.Thursday:
                                    ButtonBlocks[button].Tag += "R";
                                    break;
                                case DayOfWeek.Friday:
                                    ButtonBlocks[button].Tag += "F";
                                    break;
                            }
                        }

                        ButtonBlocks[button].Tag += "\t" + c.StartTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].StartTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].StartTime.Minutes.ToString();
                        ButtonBlocks[button].Tag += "-" + c.EndTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].EndTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].EndTime.Minutes.ToString();
                        ButtonBlocks[button].Tag += "\t(" + c.Enrollment.ToString() + " students)";
                        ButtonBlocks[button].Tag += "\n";
                    }
                    toolTip1.SetToolTip(ButtonBlocks[button], ButtonBlocks[button].Tag.ToString());
                }
                else { nulls++; }
            }


            if (_schedule.LeftoverBlocks.Count > 0)
            {
                nulls = 0;
                sortedBlocks = _schedule.LeftoverBlocks.OrderByDescending(c => c.Enrollment).ToArray();

                for (; block < _schedule.Blocks.Count() + _schedule.LeftoverBlocks.Count(); block++)
                {
                    int leftoverBlock = block - nonNullBlocks.Count();
                    int button = (block - nulls);
                    if (sortedBlocks[leftoverBlock] != null)
                    {
                        ButtonBlocks[button].Tag = sortedBlocks[leftoverBlock].ClassesInBlock.Count.ToString() + " class(es) \t" + sortedBlocks[leftoverBlock].Enrollment.ToString() + " total students\n";

                        foreach (Class c in sortedBlocks[leftoverBlock].ClassesInBlock)
                        {
                            foreach (DayOfWeek d in c.DaysMeet)
                            {
                                switch (d)
                                {
                                    case DayOfWeek.Monday:
                                        ButtonBlocks[button].Tag += "M";
                                        break;
                                    case DayOfWeek.Tuesday:
                                        ButtonBlocks[button].Tag += "T";
                                        break;
                                    case DayOfWeek.Wednesday:
                                        ButtonBlocks[button].Tag += "W";
                                        break;
                                    case DayOfWeek.Thursday:
                                        ButtonBlocks[button].Tag += "R";
                                        break;
                                    case DayOfWeek.Friday:
                                        ButtonBlocks[button].Tag += "F";
                                        break;
                                }
                            }
                            ButtonBlocks[button].Tag += "\t" + c.StartTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].StartTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].StartTime.Minutes.ToString();
                            ButtonBlocks[button].Tag += "-" + c.EndTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].EndTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].EndTime.Minutes.ToString();
                            ButtonBlocks[button].Tag += "\t(" + c.Enrollment.ToString() + " students)";
                            ButtonBlocks[button].Tag += "\n";
                        }
                        toolTip1.SetToolTip(ButtonBlocks[button], ButtonBlocks[button].Tag.ToString());
                    }
                    else { nulls++; }
                }
            }
        }

    }
}
