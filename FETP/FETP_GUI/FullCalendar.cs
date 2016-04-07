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
    public partial class FullCalendar : UserControl
    {
        private static int NUMBER_OF_DAYS = 4;
        private static int NUMBER_OF_EXAMS_PER_DAY = 10;

        private GroupBox[] Days = new GroupBox[NUMBER_OF_DAYS];
        private Button[][] Exams = new Button[NUMBER_OF_DAYS][];

        public FullCalendar()
        {
            InitializeComponent(NUMBER_OF_DAYS, NUMBER_OF_EXAMS_PER_DAY);
        }

        /// <summary>
        /// Dynamic Initializer using static ints and no event handlers
        /// </summary>
        /// <param name="NUMBER_OF_DAYS">Number of days in the schedule</param>
        /// <param name="NUMBER_OF_EXAMS_PER_DAY">Number of exam time slots per day</param>
        /// <param name="NUMBER_OF_EXAMS">Total number of exam time slots in the schedule</param>
        private void InitializeComponent(int NUMBER_OF_DAYS, int NUMBER_OF_EXAMS_PER_DAY)
        {
            #region Initialize new GUI objects
            //ComponentResourceManager resources = new ComponentResourceManager(typeof(FullCalendar));

            Days = new GroupBox[NUMBER_OF_DAYS];

            int i = 0;
            for (; i < NUMBER_OF_DAYS; i++)
            {
                Days[i] = new GroupBox();
                Exams[i] = new Button[NUMBER_OF_EXAMS_PER_DAY];
                for (int n = 0; n < NUMBER_OF_EXAMS_PER_DAY; n++)
                {
                    Exams[i][n] = new Button();
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

            #region set Dynamic GUI object properties
            //
            // Days
            //
            i = 0;
            foreach (GroupBox gb in Days)
            {
                for (int n = NUMBER_OF_EXAMS_PER_DAY - 1; n >= 0; n--)
                {
                    gb.Controls.Add(Exams[i][n]);
                }
                //gb.Dock = DockStyle.Left;
                gb.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
                gb.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                gb.ForeColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
                gb.Location = new Point((201 + 15)*i, 0);
                gb.Name = "Day " + (i + 1).ToString();
                gb.Size = new Size(200, (68 + 15) * (NUMBER_OF_EXAMS_PER_DAY + 1));
                gb.Text = gb.Name;

                i++;
            }

            //
            // Exams
            //
            int j = 0, k = 1;
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
                    y++; k++;
                }
                j++;
            }

            //
            //FullCalendar
            //
            for (i=NUMBER_OF_DAYS-1;i>=0;i--)
            {
                Controls.Add(Days[i]);
            }
            AutoScroll = true;
            Dock = DockStyle.Fill;
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

    }
}
