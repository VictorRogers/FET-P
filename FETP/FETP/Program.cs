﻿using System;
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
                Console.WriteLine("3. Test Basic Grouping");
                Console.WriteLine("4. Test Advanced Grouping (not ready)");
                Console.WriteLine("5. Test Advanced Advanced Grouping (not ready)");
                string input = Console.ReadLine();
                Console.WriteLine();
                if(input == "1")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Enrollment File Operations");
                    Console.WriteLine(STARS);

                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    // currently sorts all the data
                    //List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    //List<Class> allClasses = FETP_Controller.sortClassesByOverlappingDays(FETP_Controller.readInputDataFile(inFile));
                    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);

                    Console.WriteLine("Number of Classes: {0}", allClasses.Count);

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
                else if(input == "3")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Grouping of Classes");
                    Console.WriteLine(STARS);

                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    // currently sorts all the data
                    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
                    allClasses = FETP_Controller.sortClassesByOverlappingDays(allClasses); // sort classes how you want 
                    List<Block> groupedClasses = FETP_Controller.coalesceClassesTogether(allClasses);


                    Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
                    Console.WriteLine();

                    foreach (Block block in groupedClasses)
                    {
                        Console.WriteLine("=================================");
                        Console.WriteLine("Displaying Block");
                        Console.WriteLine("=================================");

                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying Information of Block");
                        Console.WriteLine("---------------------------------");
                        
                        block.Display();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying Information of Block");
                        Console.WriteLine("---------------------------------");


                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        block.DisplayAllClasses();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine();

                        Console.WriteLine("=================================");
                        Console.WriteLine("END Displaying Block");
                        Console.WriteLine("=================================");
                    }


                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Grouping of Classes");
                    Console.WriteLine(STARS);
                }
                else if (input == "4")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Advanced Grouping of Classes");
                    Console.WriteLine(STARS);

                    // ------------------------------------------------------------------------------------------------------
                    // Get data file information
                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    // currently sorts all the data
                    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
                    // ------------------------------------------------------------------------------------------------------

                    // ------------------------------------------------------------------------------------------------------
                    // Get contraints file information
                    inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
                    // ------------------------------------------------------------------------------------------------------


                    allClasses = FETP_Controller.sortClassesByEnrollment(allClasses);
                    List<Block> groupedClasses = FETP_Controller.coalesceClassesTogether(allClasses);
                    

                    Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
                    Console.WriteLine();

                    foreach (Block block in groupedClasses)
                    {
                        Console.WriteLine("=================================");
                        Console.WriteLine("Displaying Block");
                        Console.WriteLine("=================================");

                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying Information of Block");
                        Console.WriteLine("---------------------------------");

                        block.Display();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying Information of Block");
                        Console.WriteLine("---------------------------------");


                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        block.DisplayAllClasses();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine();

                        Console.WriteLine("=================================");
                        Console.WriteLine("END Displaying Block");
                        Console.WriteLine("=================================");
                    }

                    Console.WriteLine(STARS);
                    Console.WriteLine("END Testing Advanced Grouping of Classes");
                    Console.WriteLine(STARS);

                }
                else if(input == "5")
                {
                    Console.WriteLine(STARS);
                    Console.WriteLine("Testing Advanced Advanced Grouping of Classes");
                    Console.WriteLine(STARS);

                    // ------------------------------------------------------------------------------------------------------
                    // Get data file information
                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

                    // currently sorts all the data
                    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
                    // ------------------------------------------------------------------------------------------------------

                    // ------------------------------------------------------------------------------------------------------
                    // Get contraints file information
                    inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
                    // ------------------------------------------------------------------------------------------------------


                    allClasses = FETP_Controller.sortClassesByEnrollment(allClasses);

                    blankSchedule.blocks = FETP_Controller.createShellDays(blankSchedule);

                    List<Block> groupedClasses = FETP_Controller.coalesceClassesTogether(allClasses, blankSchedule.blocks);

                    foreach (Block block in groupedClasses)
                    {
                        Console.WriteLine("=================================");
                        Console.WriteLine("Displaying Block");
                        Console.WriteLine("=================================");

                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying Information of Block");
                        Console.WriteLine("---------------------------------");

                        block.Display();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying Information of Block");
                        Console.WriteLine("---------------------------------");


                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        block.DisplayAllClasses();
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("END Displaying all Classes in Block");
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine();

                        Console.WriteLine("=================================");
                        Console.WriteLine("END Displaying Block");
                        Console.WriteLine("=================================");
                    }

                    Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            
           
        }
       
    }
}
