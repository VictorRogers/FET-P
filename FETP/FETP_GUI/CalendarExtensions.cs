using FETP;
using FETP_GUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CalendarExtension
{
    public static class CalendarExtension
    {
        public static void labelAllScheduledBlocks(this UserControl cal, Schedule _schedule, ref Button[][] Exams)
        {
            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;
            
            int day = 0, nulls = 0;
            for (int a = 0; a < _schedule.Blocks.Count(); a++)
            {
                int block = (a - nulls) % _schedule.NumberOfTimeSlotsAvailablePerDay;
                if (_schedule.Blocks[a] != null)
                {
                    Class biggestClass = getBiggestClass(_schedule.Blocks[a]);

                    foreach (DayOfWeek d in biggestClass.DaysMeet)
                    {
                        switch (d)
                        {
                            case DayOfWeek.Monday:
                                Exams[day][block].Text += "M";
                                break;
                            case DayOfWeek.Tuesday:
                                Exams[day][block].Text += "T";
                                break;
                            case DayOfWeek.Wednesday:
                                Exams[day][block].Text += "W";
                                break;
                            case DayOfWeek.Thursday:
                                Exams[day][block].Text += "R";
                                break;
                            case DayOfWeek.Friday:
                                Exams[day][block].Text += "F";
                                break;
                        }
                    }

                    Exams[day][block].Text += "\n " + biggestClass.StartTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].StartTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].StartTime.Minutes.ToString();
                    Exams[day][block].Text += "-" + biggestClass.EndTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].EndTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].EndTime.Minutes.ToString();
                    Exams[day][block].Text += "\n (" + biggestClass.Enrollment.ToString() + ")";

                    if (block.Equals(totalPerDay)) { day++; }
                }
                else { nulls++; }
            }
        }

        private static Class getBiggestClass(Block block)
        {
            int maxEnrollment = 0;
            Class biggestClass = null;
            foreach (Class c in block.ClassesInBlock)
            {
                if (c.Enrollment > maxEnrollment)
                {
                    maxEnrollment = c.Enrollment;
                    biggestClass = c;
                }
            }
            return biggestClass;
        }
    }
}