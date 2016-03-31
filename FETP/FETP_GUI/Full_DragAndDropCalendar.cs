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
    public partial class Full_DragAndDropCalendar : Form
    {
        private static int NUMBER_OF_DAYS = 4;
        private static int NUMBER_OF_EXAMS_PER_DAY = 4;
        private static int NUMBER_OF_EXAMS = NUMBER_OF_DAYS * NUMBER_OF_EXAMS_PER_DAY;

        private GroupBox[] Days = new GroupBox[NUMBER_OF_DAYS];
        private Button[][] Exams = new Button[NUMBER_OF_DAYS][];
        private GroupBox classGroups = new GroupBox();
        private Button[] Blocks = new Button[NUMBER_OF_EXAMS];

        public Full_DragAndDropCalendar()
        {
            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY, NUMBER_OF_EXAMS);
        }

        private void InitializeComponent(int NUMBER_OF_DAYS, int NUMBER_OF_EXAMS_PER_DAY, int NUMBER_OF_EXAMS)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Full_DragAndDropCalendar));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            oneDayToolStripMenuItem = new ToolStripMenuItem();
            fullScheduleToolStripMenuItem = new ToolStripMenuItem();
            textToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();

            Days = new GroupBox[NUMBER_OF_DAYS];
            int i = 0;
            for(; i<NUMBER_OF_DAYS; i++)
            {
                Days[i] = new GroupBox();
                Exams[i] = new Button[NUMBER_OF_EXAMS_PER_DAY];
                for(int n=0; n<NUMBER_OF_EXAMS_PER_DAY; n++)
                {
                    Exams[i][n] = new Button();
                }
            }
            i = 0;
            for(; i<NUMBER_OF_EXAMS; i++)
            {
                Blocks[i] = new Button();
            }

            
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            foreach(GroupBox d in Days)
            {
                d.SuspendLayout();
            }
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.Items.AddRange(new ToolStripItem[] {
            fileToolStripMenuItem,
            viewToolStripMenuItem});
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(665, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            openToolStripMenuItem,
            saveAsToolStripMenuItem,
            exportToolStripMenuItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(114, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(114, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(114, 22);
            exportToolStripMenuItem.Text = "Export";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            oneDayToolStripMenuItem,
            fullScheduleToolStripMenuItem,
            textToolStripMenuItem});
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // oneDayToolStripMenuItem
            // 
            oneDayToolStripMenuItem.Name = "oneDayToolStripMenuItem";
            oneDayToolStripMenuItem.Size = new Size(144, 22);
            oneDayToolStripMenuItem.Text = "One Day";
            // 
            // fullScheduleToolStripMenuItem
            // 
            fullScheduleToolStripMenuItem.Name = "fullScheduleToolStripMenuItem";
            fullScheduleToolStripMenuItem.Size = new Size(144, 22);
            fullScheduleToolStripMenuItem.Text = "Full Schedule";
            // 
            // textToolStripMenuItem
            // 
            textToolStripMenuItem.Name = "textToolStripMenuItem";
            textToolStripMenuItem.Size = new Size(144, 22);
            textToolStripMenuItem.Text = "Text";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.BackColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            i = NUMBER_OF_DAYS - 1;
            for(;i>=0; i--)
            {
                splitContainer1.Panel1.Controls.Add(Days[i]);
            }
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Panel2.BackColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            splitContainer1.Panel2.Controls.Add(classGroups);
            splitContainer1.Size = new Size(665, 429);
            splitContainer1.SplitterDistance = 439;
            splitContainer1.TabIndex = 1;
            //
            // Days
            //
            i = 0;
            foreach(GroupBox gb in Days)
            {
                for(int n = NUMBER_OF_EXAMS_PER_DAY-1; n>=0; n--)
                {
                    gb.Controls.Add(Exams[i][n]);
                }
                gb.Dock = DockStyle.Left;
                gb.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                gb.ForeColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
                //gb.Location = new Point(600, 0);
                gb.Name = "Day " + (i+1).ToString();
                gb.Size = new Size(200, 68*(NUMBER_OF_EXAMS_PER_DAY+1));
                gb.Text = gb.Name;

                i++;
            }
            //
            // Exams
            //
            int j = 0, k = 1 ;
            while (j < NUMBER_OF_DAYS)
            {
                int y = 0;
                foreach (Button b in Exams[j])
                {
                    //b.Dock = DockStyle.Top;
                    b.BackColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
                    b.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(105)))), ((int)(((byte)(18)))));
                    b.FlatAppearance.BorderSize = 2;
                    b.FlatStyle = FlatStyle.Flat;
                    b.ForeColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
                    b.Location = new Point(6, (15 + ((15 + 68) * y)));
                    b.Name = "Exam Time " + (k).ToString();
                    b.Size = new Size(189, 68);
                    b.Text = b.Name;
                    b.UseVisualStyleBackColor = false;
                    y++;    k++;
                }
                j++;
            }
            //
            // classGroups
            //
            i = NUMBER_OF_EXAMS-1;
            for(; i>=0;i--)
            {
                classGroups.Controls.Add(Blocks[i]);
            }            
            classGroups.Dock = DockStyle.Top;
            classGroups.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            classGroups.ForeColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            classGroups.Location = new Point(0, 0);
            classGroups.Name = "Class Groups";
            classGroups.Size = new Size(205, (15+68)*(NUMBER_OF_EXAMS+1));
            classGroups.TabIndex = 0;
            classGroups.TabStop = false;
            classGroups.Text = "Class Groups";
            //
            // Blocks
            //
            i = 0;
            foreach(Button b in Blocks)
            {
                //b.Dock = DockStyle.Top;
                b.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
                b.FlatAppearance.BorderSize = 2;
                b.FlatStyle = FlatStyle.Flat;
                b.Location = new Point(6, (15+((15+68)*i)));
                b.Name = "Class Group " + (i+1).ToString();
                b.Size = new Size(189, 68);
                b.Text = b.Name;
                b.UseVisualStyleBackColor = false;
                i++;
            }

            // 
            // Full_DragAndDropCalendar
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            ClientSize = new Size(665, 453);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            Icon = ((Icon)(resources.GetObject("$Icon")));
            MainMenuStrip = menuStrip1;
            Name = "Full_DragAndDropCalendar";
            Text = "Final Exam Scheduler";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            foreach(GroupBox d in Days)
            {
                d.ResumeLayout(false);
            }
            classGroups.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

    }
}
