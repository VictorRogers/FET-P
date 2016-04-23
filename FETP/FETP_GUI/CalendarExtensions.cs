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
            //Block[] sortedBlocks = _schedule.Blocks;
            //sortGroupsByEnrollment(ref sortedBlocks);

            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;

            int day = 0, nulls = 0;
            int block = 0;
            for (; block < _schedule.Blocks.Count(); block++)
            {
                int button = (block - nulls);
                if (_schedule.Blocks[block] != null)
                {
                    ButtonBlocks[button].Text += "Placed\n";

                    Class biggestClass = getBiggestClass(_schedule.Blocks[block]);

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

            nulls = 0;

            for(; block < _schedule.Blocks.Count() + _schedule.LeftoverBlocks.Count(); block++)
            {
                int leftoverBlock = block - _schedule.Blocks.Count();
                int button = (block - nulls);
                if (_schedule.LeftoverBlocks[leftoverBlock] != null)
                {
                    ButtonBlocks[button].Text += "Not Placed\n";

                    Class biggestClass = getBiggestClass(_schedule.LeftoverBlocks[leftoverBlock]);

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

        ////From http://anh.cs.luc.edu/170/notes/CSharpHtml/sorting.html - modified to work with our Block class
        ////4-20-16
        //private static void sortGroupsByEnrollment(ref Block[] blocksToSort)
        //{
        //    QuickSortByEnrollment(ref blocksToSort, 0, blocksToSort.Length - 1);
        //}

        ////From http://anh.cs.luc.edu/170/notes/CSharpHtml/sorting.html - modified to work with our Block class
        ////4-20-16
        //public static void QuickSortByEnrollment(ref Block[] blocks, int l, int r)
        //{
        //    int i, j;
        //    int x;

        //    i = l;
        //    j = r;

        //    if(blocks[(l + r) / 2] != null)
        //    {
        //        x = blocks[(l + r) / 2].Enrollment; /* find pivot item */
        //    }
        //    else
        //    {
        //        x = 0;
        //    }
        //    while (true)
        //    {
        //        while (blocks[i] == null || blocks[i].Enrollment < x)
        //            i++;
        //        while (blocks[j]==null || x < blocks[j].Enrollment)
        //            j--;
        //        if (i <= j)
        //        {
        //            exchange(blocks, i, j);
        //            i++;
        //            j--;
        //        }
        //        if (i > j)
        //            break;
        //    }
        //    if (l < j)
        //        QuickSortByEnrollment(ref blocks, l, j);
        //    if (i < r)
        //        QuickSortByEnrollment(ref blocks, i, r);
        //}

        //private static void exchange(Block[] blocks, int i, int j)
        //{
        //    Block temp = blocks[i];
        //    blocks[i] = blocks[j];
        //    blocks[j] = temp;
        //}

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