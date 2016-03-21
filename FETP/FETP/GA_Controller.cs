using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        GA_Controller - Data Constants
        \**************************************************************************/ 
        const int GENERATION_SIZE = 500;
        const int MAX_GENERATION = 1000;

        const float CROSSOVER_RATE = 0.7F;
        public const float MUTATION_RATE = 0.15F;

        /**************************************************************************\
        GA_Controller - Weights
        \**************************************************************************/
        public static int WEIGHT_OVERLAPPING_CLASSES = 10; // static makes it fuction almost as const ?



        //private List<Schedule> currentGeneration;
        //private List<Schedule> topGuys;
        


        /**************************************************************************\
        GA_Controller - Methods 
        \**************************************************************************/ 
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


        //// ?
        //// need finish
        //public static Schedule generateRandomSchedule(List<Class> classes)
        //{
        //    Random rand = new Random();
        //    classes = classes.OrderBy(c => rand.Next()).Select(c => c.Model).ToList(); // randomly arrange classes



        //    Schedule schedule = new Schedule(classes);

        //}

        

        //// ?
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
