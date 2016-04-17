
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FETP_GUI
{
    public partial class SchedulePresenter : UserControl
    {
        private int NUMBER_OF_DAYS;
        private int NUMBER_OF_EXAMS_PER_DAY;
        private int NUMBER_OF_EXAMS;

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
        public SchedulePresenter(int daysNum, int examsPerDay)
        {
            NUMBER_OF_DAYS = daysNum;
            NUMBER_OF_EXAMS_PER_DAY = examsPerDay;
            NUMBER_OF_EXAMS = NUMBER_OF_DAYS * NUMBER_OF_EXAMS_PER_DAY;
            Blocks = new Button[NUMBER_OF_EXAMS];

            InitializeComponent(NUMBER_OF_EXAMS);
        }

        /// <summary>
        /// Builds the SchedulePresenter object based on total NUMBER_OF_EXAMS
        /// </summary>
        /// <param name="numOfExams">Total Number of Exams in the schedule</param>
        private void InitializeComponent(int numOfExams)
        {
            #region Initialize new GUI objects
            splitContainer1 = new SplitContainer();

            int i = 0;
            for (; i < numOfExams; i++)
            {
                Blocks[i] = new Button();
            }

            classGroups = new GroupBox();
            panel1 = new Panel();
            ((ISupportInitialize)(splitContainer1)).BeginInit();
            #endregion

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
            i = numOfExams - 1;
            for (; i >= 0; i--)
            {
                panel1.Controls.Add(Blocks[i]);
            }
            panel1.Dock = DockStyle.Fill;

            //
            // Blocks
            //
            i = 0;
            foreach (Button b in Blocks)
            {
                //b.Dock = DockStyle.Top;
                b.BackColor = Color.FromArgb(45, 12, 73);
                b.Enabled = true;
                b.FlatAppearance.BorderSize = 2;
                b.FlatStyle = FlatStyle.Flat;
                b.Location = new Point(6, (15 + ((15 + 68) * i)));
                b.Name = "Class Group " + (i + 1).ToString();
                b.Size = new Size(180, 68);
                b.Text = b.Name;
                b.UseVisualStyleBackColor = false;
                i++;
            }
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
    }
}
