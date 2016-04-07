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
        private static int NUMBER_OF_DAYS = 4;
        private static int NUMBER_OF_EXAMS_PER_DAY = 4;
        private static int NUMBER_OF_EXAMS = NUMBER_OF_DAYS * NUMBER_OF_EXAMS_PER_DAY;

        private GroupBox classGroups;

        //All the Button objects are going to need to be replaced with an extended Button object
        //Extended Button object has members Block and Label
        //Label is a drag-and-drop GUI object
        //Block is attached schedule data

            //Maybe I don't actually care about the extended Button class.
            //I can probablyjust drag and drop the actual buttons themselves and just use button.text for the label
        private Button[] Blocks = new Button[NUMBER_OF_EXAMS];

        public SchedulePresenter()
        {
            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY, NUMBER_OF_EXAMS);
        }

        private void InitializeComponent(int NUMBER_OF_DAYS, int NUMBER_OF_EXAMS_PER_DAY, int NUMBER_OF_EXAMS)
        {
            #region Initialize new GUI objects
            splitContainer1 = new SplitContainer();

            int i = 0;
            for (; i < NUMBER_OF_EXAMS; i++)
            {
                Blocks[i] = new Button();
            }

            classGroups = new GroupBox();
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
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            //splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.BackColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Panel2.BackColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            splitContainer1.Panel2.Controls.Add(classGroups);
            splitContainer1.Size = new Size(648, 466);
            splitContainer1.SplitterDistance = 427;
            splitContainer1.TabIndex = 3;
            #endregion

            #region Dynamic GUI objects
            //
            // classGroups
            //
            i = NUMBER_OF_EXAMS - 1;
            for (; i >= 0; i--)
            {
                classGroups.Controls.Add(Blocks[i]);
            }
            classGroups.Dock = DockStyle.Top;
            classGroups.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            classGroups.ForeColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            classGroups.Location = new Point(0, 0);
            classGroups.Name = "Class Groups";
            classGroups.Size = new Size(205, (15 + 68) * (NUMBER_OF_EXAMS + 1));
            classGroups.TabIndex = 0;
            classGroups.TabStop = false;
            classGroups.Text = "Class Groups";

            //
            // Blocks
            //
            i = 0;
            foreach (Button b in Blocks)
            {
                //b.Dock = DockStyle.Top;
                b.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
                b.FlatAppearance.BorderSize = 2;
                b.FlatStyle = FlatStyle.Flat;
                b.Location = new Point(6, (15 + ((15 + 68) * i)));
                b.Name = "Class Group " + (i + 1).ToString();
                b.Size = new Size(189, 68);
                b.Text = b.Name;
                b.UseVisualStyleBackColor = false;
                i++;
            }
            #endregion

            // 
            // FullCalendar
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
