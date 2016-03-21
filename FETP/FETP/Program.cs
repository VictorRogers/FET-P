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
                //Console.WriteLine("3. Test Basic Grouping");
                Console.WriteLine("4. Test Basic GA");
                Console.WriteLine("5. Test Basic GA with Generation class (broken hard)");


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
                //else if (input == "3")
                //{
                //    Console.WriteLine(STARS);
                //    Console.WriteLine("Testing Grouping of Classes");
                //    Console.WriteLine(STARS);

                //    // FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                //    // currently sorts all the data
                //    List<Class> allClasses = FETP_Controller.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");
                //    allClasses.OrderByDescending(c => c.Enrollment);
                //    //allClasses = FETP_Controller.sortClassesByOverlappingDays(allClasses); // sort classes how you want 
                //    List<Block> groupedClasses = FETP_Controller.coalesceClassesTogether(allClasses);


                //    Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
                //    Console.WriteLine();

                //    foreach (Block block in groupedClasses)
                //    {
                //        Console.WriteLine("=================================");
                //        Console.WriteLine("Displaying Block");
                //        Console.WriteLine("=================================");

                //        Console.WriteLine("---------------------------------");
                //        Console.WriteLine("Displaying Information of Block");
                //        Console.WriteLine("---------------------------------");

                //        block.Display();
                //        Console.WriteLine("---------------------------------");
                //        Console.WriteLine("END Displaying Information of Block");
                //        Console.WriteLine("---------------------------------");


                //        Console.WriteLine("---------------------------------");
                //        Console.WriteLine("Displaying all Classes in Block");
                //        Console.WriteLine("---------------------------------");
                //        block.DisplayAllClasses();
                //        Console.WriteLine("---------------------------------");
                //        Console.WriteLine("END Displaying all Classes in Block");
                //        Console.WriteLine("---------------------------------");
                //        Console.WriteLine();

                //        Console.WriteLine("=================================");
                //        Console.WriteLine("END Displaying Block");
                //        Console.WriteLine("=================================");
                //    }


                //    Console.WriteLine(STARS);
                //    Console.WriteLine("END Testing Grouping of Classes");
                //    Console.WriteLine(STARS);
                //}
                else if (input == "4") // GA 
                {
                    GA_Controller.Run();
                }
                else if (input == "5") // GA 
                {
                    Generation currentGeneration = new Generation();
                    Stopwatch stopwatch = new Stopwatch();
                    for (int i = 0; i < Generation.NUMBER_OF_GENERATIONS; i++)
                    {
                        stopwatch.Start();
                        currentGeneration = new Generation(currentGeneration.Schedules);
                        stopwatch.Stop();
                        Console.WriteLine("Time to Execute {0} generations: {1}", i + 1, stopwatch.Elapsed);
                    }
                }
                Console.WriteLine();
            }
        }
       
    }
}
