using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                Console.WriteLine("1. Reading from Enrollment File");
                Console.WriteLine("2. Reading from Constraints File");
                string input = Console.ReadLine();
                Console.WriteLine();
                if(input == "1")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Enrollment File Operations");
                    Console.WriteLine(STARS);

                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    // currently sorts all the data
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    List<Class> allClasses = FETP_Controller.sortClassesByOverlappingDays(FETP_Controller.readInputDataFile(inFile));

                    foreach (Class cl in allClasses)
                    {
                        cl.Display();

                        Console.WriteLine("Overlaps with: {0} other classes", FETP_Controller.getNumberOfOverlappingDays(allClasses, cl));
                    }

                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Enrollment File Operations");
                    Console.WriteLine(STARS);

                }
                else if(input == "2")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Constraints File Operations");
                    Console.WriteLine(STARS);

                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
                    blankSchedule.Display();
                    Console.WriteLine("Number of Timeslots Available Per Day: {0}", FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(blankSchedule));
                    Console.WriteLine("Number of Timeslots Available: {0}", FETP_Controller.getNumberOfTimeSlotsAvailable(blankSchedule));

                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Constraints File Operations");
                    Console.WriteLine(STARS);



                }
                Console.WriteLine();
            }
            
           
        }
       
    }
}
