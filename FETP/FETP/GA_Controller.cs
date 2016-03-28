using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// TODO: Although reproduction methods that are based on the use of two parents are more "biology inspired", some research[3][4] suggests that more than two "parents" generate higher quality chromosomes.


namespace FETP
{
    /**************************************************************************\
    Class: GA_Controller (Genetic Algorithm Controller)
    Description: Includes all of the primary functions needed for the genetic
                 algorithm and evolving the chromosomes to find a solution.
    \**************************************************************************/ 
    public static class GA_Controller
    {
        /**************************************************************************\
        GA_Controller - Utilities 
        \**************************************************************************/ 
        public static Stopwatch stopwatch = new Stopwatch(); // TODO: for testing purposes 


        /**************************************************************************\
        GA_Controller - Data Constants
        \**************************************************************************/ 
        // const int GENERATION_SIZE = 500;
        //private const int SIZE_OF_GENERATION = 50; // TODO: big generations take a long time
        //private const int NUMBER_OF_GENERATIONS = 1000;

        private const float CROSSOVER_RATE = 0.7F;
        public const float MUTATION_RATE = 0.15F;
       

        /**************************************************************************\
        GA_Controller - Weights
        \**************************************************************************/
        public static int WEIGHT_OVERLAPPING_CLASSES = 50; // TODO: static makes it fuction almost as const


        /**************************************************************************\
        GA_Controller - Methods 
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
        \**************************************************************************/
        public static void SetupIntialFields()
        {
            Schedule.readInputDataFile("../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv"); // TODO: throw exceptions for invalid input
            Schedule.readInputConstraintsFile("../../../../Example Data/Ben Made Constraints Sample.txt");
        }


        /**************************************************************************\
        Method: RunGeneticAlgorithm
        Description: Executes the Genetic Algorithm. For the number of 
                     generations desired to go through, advances 
                     the generation
        \**************************************************************************/
        //public static void RunGeneticAlgorithm()
        //{
        //    for (int i = 0; i < NUMBER_OF_GENERATIONS; i++)
        //    {
        //        stopwatch.Start();
        //        AdvanceGeneration();
        //        stopwatch.Stop();
        //        Console.WriteLine("Time to Execute {0} generations: {1}", i + 1, stopwatch.Elapsed);
        //    }
        //}
        //public static void RunGeneticAlgorithm2()
        //{
        //    for (int i = 0; i < NUMBER_OF_GENERATIONS; i++)
        //    {
        //        stopwatch.Start();
                
        //        stopwatch.Stop();
        //        Console.WriteLine("Time to Execute {0} generations: {1}", i + 1, stopwatch.Elapsed);
        //    }
        //}


        /**************************************************************************\
        Method: WillParentsBreed
        Description: Determines whether or not two parents will breed
        \**************************************************************************/
        public static bool WillParentsBreed(int indexOfParent1, int indexOfParent2)
        {
            return true; // TODO: Rewrite Victor
        }


        /**************************************************************************\
        Method: IntializeSeedGeneration
        Description: Creates the first generation for GA
        \**************************************************************************/
        //public static void IntializeSeedGeneration()
        //{
        //    // Intialize the current generation
        //    GA_Controller.currentGeneration = new List<Schedule>(SIZE_OF_GENERATION);

        //    // create seed generation
        //    for (int i = 0; i < GA_Controller.SIZE_OF_GENERATION; i++)
        //    {
        //        GA_Controller.currentGeneration.Add(new Schedule());
        //    }
        //}


        //// TODO: need to weight somewhere by one to avoid divide by zero if perfect population?
        ///**************************************************************************\
        //Method: BenRoutlette
        //Description: Randomly selects an index with weight from fitness scores
        //\**************************************************************************/
        //public static int BenRoutlette()
        //{
        //    int totalFitnessScoreWeight = (ComputeTotalFitnessScore()); // TODO: maybe add in one. it avoids divide by zero

        //    double randomFloat = GetRandomFloat() * totalFitnessScoreWeight;

        //    for (int i = 0; i < GA_Controller.currentGeneration.Count; i++) // TODO: current generation will shrink as more and more are moved to next generation 
        //    {
        //        randomFloat -= currentGeneration[i].FitnessScore;
        //        if (randomFloat <= 0)
        //        {
        //            return i;
        //        }
        //    }
        //    return GA_Controller.currentGeneration.Count - 1; // TODO: This point should never be reached. roundoff error?
        //}
        //public static int BenRoutlette(List<Schedule> schedules)
        //{
        //    int totalFitnessScoreWeight = (ComputeTotalFitnessScore()); // TODO: maybe add in one ? it avoids divide by zero

        //    double randomFloat = GetRandomFloat() * totalFitnessScoreWeight;

        //    for (int i = 0; i < GA_Controller.currentGeneration.Count; i++) // TODO: current generation will shrink as more and more are moved to next generation 
        //    {
        //        randomFloat -= currentGeneration[i].FitnessScore;
        //        if (randomFloat <= 0)
        //        {
        //            return i;
        //        }
        //    }
        //    return GA_Controller.currentGeneration.Count - 1; // TODO: This point should never be reached. roundoff error?
        //}


        public static Random rnd = new Random(); // this is up here to make sure we get good randoms, if it wasn't then we'd get the same random a bunch of times

        /**************************************************************************\
        Method: GetRandomFlot
        Description: Retrieves a random float between 0 and 1
        \**************************************************************************/

        public static double GetRandomFloat()
        {
            return rnd.NextDouble();
            //return new Random().NextDouble(); // TODO: we need a better implementation. numbers from this class are known to not be that random
        }

        public static int GetRandomInt(int upperRange = Int32.MaxValue, int lowerRange = 0)
        {
            return rnd.Next(lowerRange, upperRange);
            //return new Random().Next(lowerRange, upperRange);
        }


        ///**************************************************************************\
        //Method: AdvanceGeneration
        //Description: Handles crossing over of current generation to the next
        //             generation
        //\**************************************************************************/
        //public static void AdvanceGeneration()
        //{

        //    List<Schedule> nextGeneration = new List<Schedule>(GA_Controller.SIZE_OF_GENERATION);
        //    while (currentGeneration.Count > 0) // loop while there are still members in current generation // TODO: could optimze with just SIZE_OF_GENERATION and minus 2 but this is more scalable and reusable
        //    {
        //        // TODO: not sure if you're supposed to give the parents a chance to reproduce or not.
        //        // TODO: that should be handled just by selection?

        //        // get next two parents //TODO: separate out into antoher function??
        //        int indexOfParent1 = BenRoutlette();
        //        int indexOfParent2 = BenRoutlette();
        //        while (indexOfParent1 == indexOfParent2) // makes sure we have two different indexes
        //        {
        //            indexOfParent2 = BenRoutlette();
        //        }

        //        // If parents are going to breed, pass their chidlren on to next generation, else pass parents on to next generation
        //        if (WillParentsBreed(indexOfParent1, indexOfParent2))
        //        {
        //            // Add the two parents new children to the next generation
        //            nextGeneration.Add(new Schedule(currentGeneration[indexOfParent1], currentGeneration[indexOfParent2]));
        //            nextGeneration.Add(new Schedule(currentGeneration[indexOfParent2], currentGeneration[indexOfParent1]));                    
        //        }
        //        else
        //        {
        //            nextGeneration.Add(currentGeneration[indexOfParent1]);
        //            nextGeneration.Add(currentGeneration[indexOfParent2]);
        //        }

        //        // Remove the parents from the current pool
        //        if (indexOfParent1 > indexOfParent2) // this is to make sure removing one parent doesn't move the index of the second
        //        {
        //            currentGeneration.RemoveAt(indexOfParent1);
        //            currentGeneration.RemoveAt(indexOfParent2);
        //        }
        //        else
        //        {
        //            currentGeneration.RemoveAt(indexOfParent2);
        //            currentGeneration.RemoveAt(indexOfParent1);
        //        }

        //    }
            
        //    currentGeneration = nextGeneration; // Advance to the next generation
        //}
        //public static void AdvanceGeneration2()
        //{
            
        //}



        ///**************************************************************************\
        //Method: ComputeTotalFitnessScore
        //Description: Computes the total fitness score of all schedules
        //\**************************************************************************/
        //public static int ComputeTotalFitnessScore()
        //{
        //    int totalFitnessScore = 0;
        //    foreach (Schedule schedule in GA_Controller.currentGeneration)
        //    {
        //        totalFitnessScore += schedule.FitnessScore;
        //    }
        //    return totalFitnessScore;
        //}

        //public static int ComputeTotalFitnessScore(List<Schedule> schedules)
        //{
        //    int totalFitnessScore = 0;
        //    foreach (Schedule schedule in schedules)
        //    {
        //        totalFitnessScore += schedule.FitnessScore;
        //    }
        //    return totalFitnessScore;
        //}




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

        /**************************************************************************\
        Constructor: Default 
        Description: 
        \**************************************************************************/ 
        //GA_Controller()
        //{
        //}


        ///**************************************************************************\
        //Constructor: Default 
        //Description: 
        //\**************************************************************************/ 
        //GA_Controller(Class[] incomingSetClasses, int numClasses, int numExams)
        //{
        //    setClasses = new Class[numClasses];
        //    incomingSetClasses.CopyTo(setClasses, numClasses);

        //}

        //List<Schedule> generateChildren(Schedule schedule1, Schedule schedule2)
        //{

        //}


        //Schedule RouletCrossOver(Schedule schedule1, Schedule schedule2)
        //{

        //}


        //// TODO: 
        //// need finish
        //public static Schedule generateRandomSchedule(List<Class> classes)
        //{
        //    Random rand = new Random();
        //    classes = classes.OrderBy(c => rand.Next()).Select(c => c.Model).ToList(); // randomly arrange classes



        //    Schedule schedule = new Schedule(classes);

        //}

        

        //// TODO: 
        //// need finish
        //public static List<Schedule> generateRandomGeneration(List<Class> classes)
        //{
        //    List<Schedule> generation = new List<Schedule>(); // makes blank generation

        //    for (int i = 0; i < GENERATION_SIZE; i++)
        //    {
        //        generation.Add(generateRandomSchedule(classes));
        //    }

        //    return generation;
        //}

        ///**************************************************************************\
        //Method: Assign Fitness 
        //Description: Should take a chromosome as an input and output its fitness
        //             score.
        //\**************************************************************************/
        //float AssignFitness()
        //{
        //    Constraints.CheckSoftConstraints();
        //    Constraints.CheckHardConstraints();

        //    return 0.0f;
        //}


        /**************************************************************************\
        Method: Crossover 
        Description: Takes two chromosomes as an input and performs crossover on
                     the genes to create two offspring based on the crossover rate.
        \**************************************************************************/ 
        //void Crossover()
        //{
        //}


        /**************************************************************************\
        Method: Roulette 
        Description: Selects a chromosome from the population proportional to its
                     fitness score. This is used to select members from the
                     population to go on to the next generation. It does not
                     guarantee that the most fit chromosomes will be selected, but
                     it does give them a good chance of doing so.
        \**************************************************************************/ 
        //void Roulette()
        //{
        //}


        /**************************************************************************\
        Method: Mutate 
        Description: Creates a mutation in a chromosome depending on the mutation
                     rate.
        \**************************************************************************/ 
        //void Mutate()
        //{
        //}
    }
}
