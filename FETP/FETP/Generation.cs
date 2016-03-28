using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace FETP
{
    /**************************************************************************\
    Class: Generation 
    Description: 
    TODO: Add a description
    \**************************************************************************/ 
    class Generation
    {
        /**************************************************************************\
        Class: Generation 
        Section: Utilities
        \**************************************************************************/
        //Utility Data Members Here


        /**************************************************************************\
        Utility Method: Example 
        Description: This is an example header
        TODO: Remove if a utility method is added
        \**************************************************************************/

        //End Utilities Section


        /**************************************************************************\
        Class: Generation 
        Section: Data Constants 
        \**************************************************************************/
        //TODO: big generations take a long time?
        //TODO: should be divisable by 2 ?
        //TODO: we could write it to take a random number of parents for better crossover
        public const int SIZE_OF_GENERATION = 100; 
        public const int NUMBER_OF_GENERATIONS = 500;
        public const int BEN_ALL_STAR_THREAD_LIMIT = 4;

        //End Data Constants Section


        /**************************************************************************\
        Class: Generation 
        Section: Data Members 
        \**************************************************************************/
        private List<Schedule> schedules; // = new List<Schedule>(Generation.SIZE_OF_GENERATION);

        //End Data Members Section


        /**************************************************************************\
        Class: Generation 
        Sections: Properties
        TODO: Let me know if headers for properties are too excessive (VR)
        \**************************************************************************/
        /**************************************************************************\
        Property: Schedules 
        Description:
        TODO: Add a description
        \**************************************************************************/
        public List<Schedule> Schedules
        {
            get
            {
                return this.schedules;
            }
        }

        //End Properties Section


        /**************************************************************************\
        Class: Generation 
        Section: Methods
        \**************************************************************************/
        /**************************************************************************\
        Method: Overloaded Constructor 
        Description: Creates seed generation?
        TODO: Add a description 
        \**************************************************************************/
        public Generation()
        {
            // Intialize the current generation
            // this.schedules = new List<Schedule>(Generation.SIZE_OF_GENERATION);
            this.schedules = new List<Schedule>(Generation.SIZE_OF_GENERATION);
            // create seed generation
            for (int i = 0; i < Generation.SIZE_OF_GENERATION; i++)
            {
                this.schedules.Add(new Schedule());
            }
        }


        /**************************************************************************\
        Method: Overloaded Constructor 
        Description: Creates next generation
        TODO: Add a description 
        \**************************************************************************/
        public Generation(Generation generation)
        {
            // List<Schedule> nextGeneration = new List<Schedule>(GA_Controller.SIZE_OF_GENERATION);

            this.schedules = new List<Schedule>(Generation.SIZE_OF_GENERATION);
            List<Schedule> currentGeneration = new List<Schedule>(generation.Schedules); // TODO: a little confusing since currentGeneration isn't a generation object
            while (currentGeneration.Count > 0) // loop while there are still members in current generation // TODO: could optimze with just SIZE_OF_GENERATION and minus 2 but this is more scalable and reusable
            {
                // TODO: not sure if you're supposed to give the parents a chance to reproduce or not.
                // TODO: that should be handled just by selection?

                // get next two parents // TODO: separate out into antoher function??
                int indexOfParent1 = this.BenRoutlette(currentGeneration);
                int indexOfParent2 = this.BenRoutlette(currentGeneration);
                
                while (indexOfParent1 == indexOfParent2) // makes sure we have two different indexes
                {
                    indexOfParent2 = this.BenRoutlette(currentGeneration);
                }

                
                // If parents are going to breed, pass their chidlren on to next generation, else pass parents on to next generation
                if (GA_Controller.WillParentsBreed(indexOfParent1, indexOfParent2))
                {
                    // Add the two parents new children to the next generation
                    this.schedules.Add(new Schedule(currentGeneration[indexOfParent1], currentGeneration[indexOfParent2]));
                    this.schedules.Add(new Schedule(currentGeneration[indexOfParent2], currentGeneration[indexOfParent1]));
                }
                else
                {
                    this.schedules.Add(currentGeneration[indexOfParent1]);
                    this.schedules.Add(currentGeneration[indexOfParent2]);
                }
                
                // Remove the parents from the current pool
                if (indexOfParent1 > indexOfParent2) // this is to make sure removing one parent doesn't move the index of the second
                {
                    currentGeneration.RemoveAt(indexOfParent1);
                    currentGeneration.RemoveAt(indexOfParent2);
                }
                else
                {
                    currentGeneration.RemoveAt(indexOfParent2);
                    currentGeneration.RemoveAt(indexOfParent1);
                }

            }

            //currentGeneration = nextGeneration; // Advance to the next generation
        }


        /**************************************************************************\
        Method: Overloaded Constructor 
        Description: ?????!??? 
        TODO: Add a description 
        \**************************************************************************/
        public Generation(List<Schedule> inSchedules)
        {
            this.schedules = inSchedules;
        }

        public bool IsEmpty()
        {
            return this.schedules.Count <= 0;
        }


        /**************************************************************************\
        Method: BenRoutlette
        Description: Randomly selects an index with weight from fitness scores
        TODO: need to weight somewhere by one to avoid divide by zero if perfect population?
        \**************************************************************************/
        public int BenRoutlette(List<Schedule> schedules)
        {
            double totalFitnessScoreWeight = (this.ComputeTotalFitnessScore(schedules)); // TODO: maybe add in one ? it avoids divide by zero

            double randomFloat = GA_Controller.GetRandomFloat() * totalFitnessScoreWeight;
            for (int i = 0; i < schedules.Count; i++) // TODO: current generation will shrink as more and more are moved to next generation 
            {

                randomFloat -= schedules[i].FitnessScore;
                if (randomFloat <= 0)
                {
                    return i;
                }
            }
            Console.WriteLine("pls");
            return this.schedules.Count - 1; // TODO: This point should never be reached. roundoff error? ? it could fail on empty schedule
        }


        /**************************************************************************\
        Method: ComputeTotalFitnessScore
        Description: Computes the total fitness score of all schedules
        \**************************************************************************/
        public double ComputeTotalFitnessScore(List<Schedule> schedules)
        {
            double totalFitnessScore = 0;
            foreach (Schedule schedule in schedules)
            {
                totalFitnessScore += schedule.FitnessScore;
            }
            return totalFitnessScore;
        }


        /**************************************************************************\
        Method: OrderByFitnessScore 
        Description: !?!? 
        TODO: Add a description 
        \**************************************************************************/
        public void OrderByFitnessScore()
        {
            this.schedules = this.schedules.OrderByDescending(c => c.FitnessScore).ToList();
        }


        /**************************************************************************\
        Method: GetMostFit 
        Description: !?!? 
        TODO: Add a description 
        \**************************************************************************/
        public Schedule GetMostFit()
        {
            OrderByFitnessScore();
            return this.Schedules[0];
        }


        /**************************************************************************\
        Method: GetWorstFit 
        Description: !?!? 
        TODO: Add a description 
        \**************************************************************************/
        public Schedule GetWorstFit()
        {
            OrderByFitnessScore();
            return this.Schedules[this.Schedules.Count-1];
        }

        //End Methods Section


        /**************************************************************************\
        Class: Generation 
        Section: Overloaded Operators 
        \**************************************************************************/
        /**************************************************************************\
        Operator: ==
        Description: This is an example 
        \**************************************************************************/

        //End Overloaded Operators Section
    }
}
