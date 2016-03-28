using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FETP
{
    /**************************************************************************\
    Class: GA_Controller (Genetic Algorithm Controller)
    Description: Includes all of the primary functions needed for the genetic
                 algorithm and evolving the chromosomes to find a solution.
    TODO: Although reproduction methods that are based on the use of two 
          parents are more "biology inspired", some research[3][4] suggests 
          that more than two "parents" generate higher quality chromosomes.
    \**************************************************************************/ 
    public static class GA_Controller
    {
        /**************************************************************************\
        Class: GA_Controller
        Section: Utilities
        \**************************************************************************/ 
        //TODO: For testing purposes
        public static Stopwatch stopwatch = new Stopwatch();

        //This is up here to make sure we get good randoms, if it wasn't then we'd
        //get the same random a bunch of times
        public static Random rnd = new Random();


        /**************************************************************************\
        Utility Method: GetRandomFloat
        Description: Retrieves a random float between 0 and 1
        \**************************************************************************/
        public static double GetRandomFloat()
        {
            return rnd.NextDouble();
            //return new Random().NextDouble(); // TODO: we need a better implementation. numbers from this class are known to not be that random
        }


        /**************************************************************************\
        Utility Method: GetRandomInt
        Description:
        TODO: Add an accurate description - The old one was for GetRandomFloat
        \**************************************************************************/
        public static int GetRandomInt(int upperRange = Int32.MaxValue, int lowerRange = 0)
        {
            return rnd.Next(lowerRange, upperRange);
            //return new Random().Next(lowerRange, upperRange);
        }

        //End Utilities Section


        /**************************************************************************\
        Class: GA_Controller
        Section: Data Constants
        \**************************************************************************/ 
        // const int GENERATION_SIZE = 500;
        //private const int SIZE_OF_GENERATION = 50; // TODO: big generations take a long time
        //private const int NUMBER_OF_GENERATIONS = 1000;

        private const float CROSSOVER_RATE = 0.7F;
        public const float MUTATION_RATE = 0.15F;
       
        //End Data Constants Section

        /**************************************************************************\
        Class: GA_Controller
        Section: Data Members
        \**************************************************************************/
        //TODO: Static makes this function almost as a const
        //TODO: If this is not a const then it probably still shouldn't be all caps (VR)
        public static int WEIGHT_OVERLAPPING_CLASSES = 50;

        //End Data Members Section


        /**************************************************************************\
        Class: GA_Controller
        Section: Methods
        \**************************************************************************/
        /**************************************************************************\
        Method: BenAllStartRun 
        Description: 
        TODO: Add a description for this method 
        \**************************************************************************/
        public static void BenAllStartRun()
        {
            SetupIntialFields();

            List<Schedule> allStars = new List<Schedule>();

            Stopwatch newStopwatch = new Stopwatch();
            newStopwatch.Start();

            object benLock = new object();

            //TODO: Kill or fix 
            //for (int i = 0; i < Generation.SIZE_OF_GENERATION; i++)
            //{
            //    Generation generation = new Generation(); // sets up intial generation
            //                                              //Console.WriteLine("Starting GA: {0}", index);
            //    for (int j = 0; j < Generation.NUMBER_OF_GENERATIONS; j++) // TODO: is i the same for all loops
            //    {
            //        generation = new Generation(generation);
            //    }

            //    //Console.WriteLine("Time to Execute generation number {0}: {1}", index + 1, newStopwatch.Elapsed);
            //    //lock (benLock)
            //    //{
            //    Console.WriteLine("finished generation: " + i);
            //    allStars.Add(generation.GetMostFit());
            //}


            Parallel.For(0, Generation.SIZE_OF_GENERATION, new ParallelOptions { MaxDegreeOfParallelism = Generation.BEN_ALL_STAR_THREAD_LIMIT }, index =>
            {
                Generation generation = new Generation(); // sets up intial generation
                Console.WriteLine("Starting GA: {0}", index+1);
                for (int i = 0; i < Generation.NUMBER_OF_GENERATIONS; i++) // TODO: is i the same for all loops?
                {
                    generation = new Generation(generation);
                }

                Console.WriteLine("Time to Execute GA {0}: {1}", index + 1, newStopwatch.Elapsed);
                //lock (benLock)
                //{
                allStars.Add(generation.GetMostFit());
                //}
            });

            //TODO: Kill or fix
            //for(int i = 0; i < SIZE_OF_GENERATION; i++)
            //{
            //    Run();
            //    allStars.Add(currentGeneration[0]);
            //}

            // Do All Star run
            Console.WriteLine("Starting allstar run");
            Generation lastGen = new Generation(allStars);
            lastGen.OrderByFitnessScore();

            foreach(Schedule sch in lastGen.Schedules)
            {
                CheckSchedule(sch); // TODO: could be that some groups have less overlapping classes even though more clean blocks?
            }

            //TODO: Kill or fix
            //Console.WriteLine("Displaying worst fit schedule");
            //Console.WriteLine();
            //lastGen.GetWorstFit().DisplayBlocks();
            //Console.WriteLine("Displaying most fit schedule");
            //Console.WriteLine();

            //CheckSchedule(lastGen.GetMostFit());

            //Console.WriteLine("Displaying most fit schedules");
            //Console.WriteLine();
            //for (int i = 0; i < 5; i++)
            //{
            //    CheckSchedule(lastGen.Schedules[i]); // should already be sorted
            //}
        }

        
        /**************************************************************************\
        Method: Run
        Description: Basic Testing Driver for GA
        TODO: The description for this needs more detail
        \**************************************************************************/
        public static void Run()
        {
            Console.WriteLine("Begining GA\n");

            // Get input
            SetupIntialFields();

            // Intialize First Generation
            stopwatch.Restart();

            Generation generation = new Generation();

            stopwatch.Stop();
            Console.WriteLine("Time to Create Seed Generation: {0}", stopwatch.Elapsed);
            stopwatch.Reset();

            // Run GA
            for (int i = 0; i < Generation.NUMBER_OF_GENERATIONS; i++)
            {
                stopwatch.Start();
                generation = new Generation(generation);
                stopwatch.Stop();
                Console.WriteLine("Time to Execute {0} generations: {1}", i + 1, stopwatch.Elapsed);
            }

            Console.WriteLine("Displaying worst fit schedule");
            Console.WriteLine();
            //generation.GetMostFit().DisplayBlocks();
            CheckSchedule(generation.GetWorstFit());

            Console.WriteLine("Displaying most fit schedules");
            Console.WriteLine();
            for(int i = 0; i < 5; i++)
            {
                CheckSchedule(generation.Schedules[i]); // should already be sorted
            }
            
            //TODO: Kill or fix
            //currentGeneration.OrderByDescending(c => c.FitnessScore).ToList();

            //Console.WriteLine();
            //currentGeneration[0].DisplayBlocks();
            //CheckSchedule(currentGeneration[0]);

            //Console.WriteLine("Begining GA\n");

            //    // Get input
            //    SetupIntialFields();

            //    // Intialize First Generation
            //    stopwatch.Restart();

            //    IntializeSeedGeneration();

            //    stopwatch.Stop();
            //    Console.WriteLine("Time to Create Seed Generation: {0}", stopwatch.Elapsed);
            //    stopwatch.Reset();

            //    // Run GA
            //    RunGeneticAlgorithm();

            //    Console.WriteLine("Displaying most fit schedule");
            //    currentGeneration.OrderByDescending(c => c.FitnessScore).ToList();
                
            //    Console.WriteLine();
            //    currentGeneration[0].DisplayBlocks();
            //    CheckSchedule(currentGeneration[0]);
        }


        /**************************************************************************\
        Method: SetupInitialFields 
        Description: 
        TODO: This functionality needs to be overhauled to take in a variable path
        \**************************************************************************/
        public static void SetupIntialFields()
        {
            //TODO: Throw exceptions for invalid input
            Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");
            Schedule.readInputConstraintsFile("../../../../Example Data/Ben Made Constraints Sample.txt");
        }


        /**************************************************************************\
        Method: WillParentsBreed
        Description: Determines whether or not two parents will breed
        \**************************************************************************/
        public static bool WillParentsBreed(int indexOfParent1, int indexOfParent2)
        {
            return true; // TODO: Rewrite Victor
        }


        /**************************************************************************\
        Method: SaveMostFitSchedule
        Description: Saves the most fit schedule to file
        \**************************************************************************/
        public static void SaveMostFitSchedule()
        {
            // TODO: write
        }


        /**************************************************************************\
        Method: SaveGeneration
        Description: Saves the current generation to file
        \**************************************************************************/
        public static void SaveGeneration()
        {
            // TODO: write
        }


        /**************************************************************************\
        Method: CheckSchedule 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public static void CheckSchedule(Schedule schedule) // TODO: for testing
        {
            Console.WriteLine("\n***********************************************\n");
            Console.WriteLine("How good is this Schedule?... Let's find out!!!");
            Console.WriteLine("\n************************************************\n");

            //Console.WriteLine("Printing Basic information on Schedule");
            //Console.WriteLine("**************************************");
            //Schedule.Display();
            //Console.WriteLine();

            Console.WriteLine("Number of Blocks: {0}", Schedule.NumberOfTimeSlotsAvailable);

            // Check for bad blocks
            int failingBlocks = 0;
            foreach(Block block in schedule.Blocks)
            {
                if(block.AreThereAnyNonOverlappingClasses)
                {
                    failingBlocks++;
                }
            }
            Console.WriteLine("Number of Blocks that do *NOT* have overlapping Classes: {0}", Schedule.NumberOfTimeSlotsAvailable - failingBlocks);
        }

        //End Methods Section


        /**************************************************************************\
        Class: GA_Controller 
        Section: Overloaded Operators 
        \**************************************************************************/
        /**************************************************************************\
        Operator: ==
        Description: This is an example 
        \**************************************************************************/

        //End Overloaded Operators Section
    }
}
