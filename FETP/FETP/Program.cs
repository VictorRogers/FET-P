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
                Console.WriteLine("1. Reading from Enrollment File");
                Console.WriteLine("2. Reading from Constraints File");
                Console.WriteLine("3. Test Basic GA");
                Console.WriteLine("4. Test Ben's Allstar GA    <---- will devour CPU like ice cream on a hot day"); // ? crashes every now and then. theres a bug somewhere...


                string input = Console.ReadLine();
                Console.WriteLine();
                if (input == "1")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Enrollment File Operations");
                    Console.WriteLine(STARS);


                    // currently sorts all the data
                    //List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    //List<Class> allClasses = FETP_Controller.sortClassesByOverlappingDays(FETP_Controller.readInputDataFile(inFile));
                    Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    Console.WriteLine("Number of Classes: {0}", Schedule.AllClasses.Count);
                    Console.WriteLine();

                    foreach (Class cl in Schedule.AllClasses)
                    {

                        cl.Display();

                        // Console.WriteLine("Overlaps with: {0} other classes", FETP_Controller.getNumberOfOverlappingDays(allClasses, cl));
                    }

                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Enrollment File Operations");
                    Console.WriteLine(STARS);

                }
                else if (input == "2")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Constraints File Operations");
                    Console.WriteLine(STARS);


                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    Schedule.readInputConstraintsFile("../../../../Example Data/Ben Made Constraints Sample.txt");
                    Schedule.Display();
                    Console.WriteLine("Number of Timeslots Available Per Day: {0}", Schedule.NumberOfTimeSlotsAvailablePerDay);
                    Console.WriteLine("Number of Timeslots Available: {0}", Schedule.NumberOfTimeSlotsAvailable);

                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Constraints File Operations");
                    Console.WriteLine(STARS);



                }
                
                else if (input == "3") // GA 
                {
                    //while(true)
                    GA_Controller.Run();
                }
                else if (input == "4")
                {
                   // while(true)
                    GA_Controller.BenAllStartRun();
                }
                
                Console.WriteLine();
            }
        }
       
    }
}
