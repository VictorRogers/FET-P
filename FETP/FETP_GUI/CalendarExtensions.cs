using FETP;
using FETP_GUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CalendarExtension
{
    public static class CalendarExtension
    {
        public static void labelAllListedBlocks(this UserControl cal, Schedule _schedule, ref Button[] ButtonBlocks)
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
            for(int j=0; _schedule.Blocks[j] != null; j++)
            {
                nonNullBlocks[j] = _schedule.Blocks[j];
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
                    ButtonBlocks[button].Text += "Placed\n";

                    Class biggestClass = getBiggestClass(sortedBlocks[block]);

                    foreach (DayOfWeek d in biggestClass.DaysMeet)
                    {
                        switch (d)
                        {
                            case DayOfWeek.Monday:
                                ButtonBlocks[button].Text += "M";
                                break;
                            case DayOfWeek.Tuesday:
                                ButtonBlocks[button].Text += "T";
                                break;
                            case DayOfWeek.Wednesday:
                                ButtonBlocks[button].Text += "W";
                                break;
                            case DayOfWeek.Thursday:
                                ButtonBlocks[button].Text += "R";
                                break;
                            case DayOfWeek.Friday:
                                ButtonBlocks[button].Text += "F";
                                break;
                        }
                    }

                    ButtonBlocks[button].Text += "\n " + biggestClass.StartTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].StartTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].StartTime.Minutes.ToString();
                    ButtonBlocks[button].Text += "-" + biggestClass.EndTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].EndTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].EndTime.Minutes.ToString();
                    ButtonBlocks[button].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";
                }
                else { nulls++; }
            }

            if(_schedule.LeftoverBlocks.Count > 0)
            {
                nulls = 0;
                sortedBlocks = _schedule.LeftoverBlocks.OrderByDescending(c => c.Enrollment).ToArray();

                for (; block < _schedule.Blocks.Count() + _schedule.LeftoverBlocks.Count(); block++)
                {
                    int leftoverBlock = block - nonNullBlocks.Count();
                    int button = (block - nulls);
                    if (sortedBlocks[leftoverBlock] != null)
                    {
                        ButtonBlocks[button].Text += "Not Placed\n";

                        Class biggestClass = getBiggestClass(sortedBlocks[leftoverBlock]);

                        foreach (DayOfWeek d in biggestClass.DaysMeet)
                        {
                            switch (d)
                            {
                                case DayOfWeek.Monday:
                                    ButtonBlocks[button].Text += "M";
                                    break;
                                case DayOfWeek.Tuesday:
                                    ButtonBlocks[button].Text += "T";
                                    break;
                                case DayOfWeek.Wednesday:
                                    ButtonBlocks[button].Text += "W";
                                    break;
                                case DayOfWeek.Thursday:
                                    ButtonBlocks[button].Text += "R";
                                    break;
                                case DayOfWeek.Friday:
                                    ButtonBlocks[button].Text += "F";
                                    break;
                            }
                        }

                        ButtonBlocks[button].Text += "\n " + biggestClass.StartTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].StartTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].StartTime.Minutes.ToString();
                        ButtonBlocks[button].Text += "-" + biggestClass.EndTime.ToString(); // _schedule.Blocks[a].ClassesInBlock[0].EndTime.Hours.ToString() + ":" + _schedule.Blocks[a].ClassesInBlock[0].EndTime.Minutes.ToString();
                        ButtonBlocks[button].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";
                    }
                    else { nulls++; }
                }
            }
        }
        
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
                    Exams[day][block].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";

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