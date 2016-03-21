using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace FETP
{
    class Generation
    {
        private List<Schedule> schedules;

        public const int GENERATION_SIZE = 100; // ? big generations take a long time
        public const int NUMBER_OF_GENERATIONS = 4;

        private Stopwatch stopWatch = new Stopwatch();

        public List<Schedule> Schedules
        {
            get
            {
                return schedules;
            }
        }

        // create seed generation
        public Generation()
        {
            this.schedules = new List<Schedule>(Generation.GENERATION_SIZE);
            for (int i = 0; i < Generation.GENERATION_SIZE; i++)
            {
                this.schedules.Add(new Schedule());
            }
        }

        // creates next generation
        public Generation(List<Schedule> currentGeneration)
        {
            this.schedules = new List<Schedule>(Generation.GENERATION_SIZE);
            while (currentGeneration.Count > 0) // loop while there are still members in current generation // ? could optimze with just MAX_Generation and minus 2 but this is more scalable and reusable
            {
                // ? not sure if you're supposed to give the parents a chance to reproduce or not.
                // ? that should be handled just by selection?

                // get next two parents // separate out into antoher function??
                int indexOfParent1 = BenRoutlette(currentGeneration);
                int indexOfParent2 = BenRoutlette(currentGeneration);
                while (indexOfParent1 == indexOfParent2) // makes sure we have two different indexes
                {
                    indexOfParent2 = BenRoutlette(currentGeneration);
                }



                // Add the two parents new children to the next generation
                this.schedules.Add(new Schedule(currentGeneration[indexOfParent1], currentGeneration[indexOfParent2]));
                this.schedules.Add(new Schedule(currentGeneration[indexOfParent2], currentGeneration[indexOfParent1]));

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
            

        }


        // ? need to weight somewhere by one to avoid divide by zero if perfect population?
        /**************************************************************************\
        Method: BenRoutlette
        Description: Randomly selects an index with weight from fitness scores
        \**************************************************************************/
        public int BenRoutlette(List<Schedule> currentGeneration)
        {
            int totalFitnessScoreWeight = (ComputeTotalFitnessScore(currentGeneration)); // maybe add in one ? it avoids divide by zero

            double randomFloat = GetRandomFloat() * totalFitnessScoreWeight;

            for (int i = 0; i < currentGeneration.Count; i++) // ? current generation will shrink as more and more are moved to next generation 
            {
                randomFloat -= currentGeneration[i].FitnessScore;
                if (randomFloat <= 0)
                {
                    return i;
                }
            }
            return currentGeneration.Count - 1; // ? This point should never be reached. roundoff error?
        }

        /**************************************************************************\
        Method: GetRandomFlot
        Description: Retrieves a random float between 0 and 1
        \**************************************************************************/
        public static double GetRandomFloat()
        {
            return new Random().NextDouble(); // ? we need a better implementation. numbers from this class are known to not be that random
        }

        public int ComputeTotalFitnessScore(List<Schedule> currentGeneration)
        {
            int totalFitnessScore = 0;
            foreach (Schedule schedule in currentGeneration)
            {
                totalFitnessScore += schedule.FitnessScore;
            }
            return totalFitnessScore;
        }
    }
}
