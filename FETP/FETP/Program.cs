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
                Console.WriteLine("5. Basic Grouping");


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
                else if (input == "5")
                {
                    Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");
                    Console.WriteLine("Number of Classes: {0}", Schedule.AllClasses.Count);
                    Console.WriteLine();

                    List<Block> blocks = FETP_Controller.GroupClasses(Schedule.AllClasses.OrderByDescending(c => c.Enrollment).ToList());
                    int smallest = blocks.Count;

                    int variance = FETP_Controller.ComputeVarianceOfBlocks(blocks);



                    object benlock = new object();

                    object benlock2 = new object();
                    int foundNewCount = 0;
                    int countSearched = 0;
                    Stopwatch newTimer = new Stopwatch();
                    newTimer.Start();

                    Console.WriteLine("Searched: {0}: ", countSearched);
                    Console.WriteLine("New Variance: {0}: ", variance);
                    Console.WriteLine("Count: {0}: ", foundNewCount);
                    Console.WriteLine("Number of Blocks: {0}", blocks.Count);
                    Console.WriteLine("**********************");

                    Parallel.For(0, 1000000, new ParallelOptions { MaxDegreeOfParallelism = Generation.BEN_ALL_STAR_THREAD_LIMIT }, index =>
                    {
                        lock(benlock2)
                        {
                            countSearched++;
                        }
                        List<Block> newblocks = FETP_Controller.GroupClasses(Schedule.AllClasses.OrderBy(c => GA_Controller.GetRandomInt()).ToList());

                      lock(benlock)
                        {
                            if (newblocks.Count < smallest)
                            {
                                foundNewCount++;
                                Console.WriteLine("Searched: {0}: ", countSearched);
                                Console.WriteLine("Count: {0}: ", foundNewCount);
                                Console.WriteLine("Number of Blocks: {0}", newblocks.Count);
                                smallest = newblocks.Count;
                                blocks = newblocks;

                                variance = FETP_Controller.ComputeVarianceOfBlocks(blocks);

                                Console.WriteLine("New Variance: {0}: ", variance);
                                Console.WriteLine("**********************");
                            }
                            else if(newblocks.Count == smallest)
                            {
                                
                                int newvariance = FETP_Controller.ComputeVarianceOfBlocks(newblocks);
                                if(newvariance > variance)
                                {
                                    foundNewCount++;
                                    Console.WriteLine("Searched: {0}: ", countSearched);
                                    Console.WriteLine("New Variance: {0}: ", newvariance);
                                    Console.WriteLine("Count: {0}: ", foundNewCount);
                                    Console.WriteLine("Number of Blocks: {0}", newblocks.Count);
                                    //smallest = newblocks.Count;
                                    variance = newvariance;
                                    blocks = newblocks;
                                    Console.WriteLine("**********************");
                                }

                                 
                            }

                        }






                    });

                    newTimer.Stop();
                    Console.WriteLine(newTimer.Elapsed);
                    //for(int i = 0; i < 10000000; i++)
                    //{
                    //    blocks = FETP_Controller.GroupClasses(Schedule.AllClasses.OrderBy(c => GA_Controller.GetRandomInt()).ToList());
                    //    if (blocks.Count < smallest)
                    //    {
                    //        Console.WriteLine("Number of Blocks: {0}", blocks.Count);
                    //        smallest = blocks.Count;
                    //    }
                    //}

                    Console.WriteLine("******DONE************");
                    Console.WriteLine("Searched: {0}: ", countSearched);
                    Console.WriteLine("New Variance: {0}: ", variance);
                    Console.WriteLine("Count: {0}: ", foundNewCount);
                    Console.WriteLine("Number of Blocks: {0}", blocks.Count);
                    Console.WriteLine("**********************");
                    blocks = blocks.OrderByDescending(c => c.Enrollment).ToList();
                    foreach (Block block in blocks)
                    {
                        block.Display();
                        block.DisplayAllClasses();
                    }
                }
                
                Console.WriteLine();
            }
        }
       
    }
}
