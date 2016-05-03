using FETP;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CalendarExtension
{
    public static class CalendarExtension
    {
        private static Block[] sortedScheduled;
        private static Block[] sortedUnscheduled;

        /// <summary>
        /// Set text of all Buttons in the Sorted list panel
        /// </summary>
        /// <param name="cal">Schedule Presenter containing sorted list panel</param>
        /// <param name="_schedule">Schedule object containing class blocks to sort</param>
        /// <param name="ButtonBlocks">Set of Button objects in the Schedule Presenter</param>
        //Amy Brown
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

            sortedScheduled = nonNullBlocks.OrderByDescending(b => b.Enrollment).ToArray();
            
            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;

            nulls = 0;
            int block = 0;
            for (; block < sortedScheduled.Length; block++)
            {
                int button = (block - nulls);
                if (sortedScheduled[block] != null)
                {
                    ButtonBlocks[button].Text += ButtonBlocks[button].Name + " - Placed\n";

                    Class biggestClass = getBiggestClass(sortedScheduled[block]);

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

                    ButtonBlocks[button].Text += "\n " + biggestClass.StartTime.ToString();
                    ButtonBlocks[button].Text += "-" + biggestClass.EndTime.ToString();
                    ButtonBlocks[button].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";
                }
                else { nulls++; }
            }

            if(_schedule.LeftoverBlocks.Count > 0)
            {
                nulls = 0;
                sortedUnscheduled = _schedule.LeftoverBlocks.OrderByDescending(c => c.Enrollment).ToArray();

                for (; block < _schedule.Blocks.Count() + _schedule.LeftoverBlocks.Count(); block++)
                {
                    int leftoverBlock = block - nonNullBlocks.Count();
                    int button = (block - nulls);
                    if (sortedUnscheduled[leftoverBlock] != null)
                    {
                        ButtonBlocks[button].Text += ButtonBlocks[button].Name + " - Not Placed\n";

                        Class biggestClass = getBiggestClass(sortedUnscheduled[leftoverBlock]);

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

                        ButtonBlocks[button].Text += "\n " + biggestClass.StartTime.ToString();
                        ButtonBlocks[button].Text += "-" + biggestClass.EndTime.ToString();
                        ButtonBlocks[button].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";
                    }
                    else { nulls++; }
                }
            }
        }

        //------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Set text of all Buttons in the given Calendar view
        /// </summary>
        /// <param name="cal">Calendar view UserControl containing Buttons</param>
        /// <param name="_schedule">Schedule object containing scheduled classes</param>
        /// <param name="Exams">Set of Button objects in the Calendar View</param>
        //Amy Brown
        public static void labelAllScheduledBlocks(this UserControl cal, Schedule _schedule, ref Button[][] Exams)
        {
            int totalPerDay = _schedule.NumberOfTimeSlotsAvailablePerDay - 1;
            
            int day = 0, nulls = 0;
            for (int a = 0; a < _schedule.Blocks.Count(); a++)
            {
                int block = (a - nulls) % _schedule.NumberOfTimeSlotsAvailablePerDay;
                if (_schedule.Blocks[a] != null)
                {

                    for (int i = 0; i < sortedScheduled.Count(); i++)
                    {
                        if (_schedule.Blocks[a].Equals(sortedScheduled[i]))
                        {
                            Exams[day][block].Text += "Group #" + (i+1).ToString() + "\n";
                        }
                    }

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

                    Exams[day][block].Text += "\n " + biggestClass.StartTime.ToString();
                    Exams[day][block].Text += "-" + biggestClass.EndTime.ToString();
                    Exams[day][block].Text += "\n (" + biggestClass.Enrollment.ToString() + " students)";

                    if (block.Equals(totalPerDay)) { day++; }
                }
                else { nulls++; }
            }
        }

        /// <summary>
        /// Find the Class with the largest enrollment in the given Block
        /// </summary>
        /// <param name="block">Block to search</param>
        /// <returns>Class with the largest enrollment in the given Block</returns>
        //Amy Brown
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