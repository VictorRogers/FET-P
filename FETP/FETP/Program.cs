using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace FETP
{
    class Program
    {
        public const string STARS = "*******************************************";
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Would you like to test");
                //Console.WriteLine("1. Reading from Enrollment File");
                //Console.WriteLine("2. Reading from Constraints File");
                //Console.WriteLine("3. Basic Grouping");
                Console.WriteLine("1. Full Grouping and Scheduling");

                string input = Console.ReadLine();
                Console.WriteLine();
                
                if (input == "1")
                {
                    Schedule schedule = new Schedule("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv", "../../../../Example Data/Ben Made Constraints Sample.txt");
                    schedule.Display();
                    schedule.DisplayBlocks();

                    Console.WriteLine("Would you like to save the schedule? (Y / N)");
                    string saveDecision = Console.ReadLine();
                    if (saveDecision == "Y" || saveDecision == "y")
                    {
                        schedule.SaveScheduleToXML(AppDomain.CurrentDomain.BaseDirectory + "testScheduleSave.xml");
                    }
                }

                if (input == "2")
                {

                }

                Console.WriteLine();
            }
        }
       
    }
}
