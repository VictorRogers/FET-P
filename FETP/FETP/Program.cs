﻿using System;
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
                Console.WriteLine("3. Basic Grouping");

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
                else if (input == "3") 
                {
                    Schedule.readInputDataFile("../../../../Example Data/Fall 2014 Total Enrollments by Meeting times.csv");
                    Console.WriteLine("Number of Classes: {0}", Schedule.AllClasses.Count);

                    List<Block> blocks = FETP_Controller.GroupClasses(Schedule.AllClasses.OrderBy(c => 
                                         FETP_Controller.getNumberOfOverlappingClasses(Schedule.AllClasses, c)).ThenBy(c => 
                                         c.Enrollment).ToList());
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

                    //Parallel.For(0, 100000, new ParallelOptions { MaxDegreeOfParallelism = FETP_Controller.THREAD_LIMIT }, index =>
                    //{
                    //    lock (benlock2)
                    //    {
                    //        countSearched++;
                    //    }
                    //    List<Block> newblocks = FETP_Controller.GroupClasses(Schedule.AllClasses.OrderBy(c => FETP_Controller.GetRandomInt()).ToList());

                    //    lock (benlock)
                    //    {
                    //        if (newblocks.Count < smallest)
                    //        {
                    //            foundNewCount++;
                    //            Console.WriteLine("Searched: {0}: ", countSearched);
                    //            Console.WriteLine("Count: {0}: ", foundNewCount);
                    //            Console.WriteLine("Number of Blocks: {0}", newblocks.Count);
                    //            smallest = newblocks.Count;
                    //            blocks = newblocks;

                    //            variance = FETP_Controller.ComputeVarianceOfBlocks(blocks);

                    //            Console.WriteLine("New Variance: {0}: ", variance);
                    //            Console.WriteLine("**********************");
                    //        }
                    //    }
                    //});

                    //newTimer.Stop();
                    //Console.WriteLine(newTimer.Elapsed);
                    //Console.WriteLine("******DONE************");
                    //Console.WriteLine("Searched: {0}: ", countSearched);
                    //Console.WriteLine("New Variance: {0}: ", variance);
                    //Console.WriteLine("Count: {0}: ", foundNewCount);
                    //Console.WriteLine("Number of Blocks: {0}", blocks.Count);
                    //Console.WriteLine("**********************");
                    blocks = blocks.OrderByDescending(c => c.Enrollment).ToList();
                    foreach (Block block in blocks)
                    {
                        Console.WriteLine("****START OF BLOCK****");
                        block.Display();
                        Console.WriteLine("\nCLASSES IN BLOCK\n*****");
                        block.DisplayAllClasses();
                        Console.WriteLine("****END OF BLOCK****\n");
                    }
                }
                Console.WriteLine();
            }
        }
       
    }
}
