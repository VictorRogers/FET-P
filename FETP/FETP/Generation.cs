﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace FETP
{
    class Generation
    {
        private List<Schedule> schedules; // = new List<Schedule>(Generation.SIZE_OF_GENERATION);
        public List<Schedule> Schedules
        {
            get
            {
                return this.schedules;
            }
        }

        public const int SIZE_OF_GENERATION = 30; // ? big generations take a long time ? should be divisable by 2 ? we could write it to take a random number of parents for better crossover
        public const int NUMBER_OF_GENERATIONS = 100;

        // creates seed generation
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

        // creates next gen
        public Generation(Generation generation)
        {
            // List<Schedule> nextGeneration = new List<Schedule>(GA_Controller.SIZE_OF_GENERATION);

            this.schedules = new List<Schedule>(Generation.SIZE_OF_GENERATION);
            List<Schedule> currentGeneration = new List<Schedule>(generation.Schedules); // ? a little confusing since currentGeneration isn't a generation object
            while (currentGeneration.Count > 0) // loop while there are still members in current generation // ? could optimze with just SIZE_OF_GENERATION and minus 2 but this is more scalable and reusable
            {
                // ? not sure if you're supposed to give the parents a chance to reproduce or not.
                // ? that should be handled just by selection?

                // get next two parents // separate out into antoher function??
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

        public Generation(List<Schedule> inSchedules)
        {
            this.schedules = inSchedules;
        }

        public bool IsEmpty()
        {
            return this.schedules.Count <= 0;
        }

        // ? need to weight somewhere by one to avoid divide by zero if perfect population?
        /**************************************************************************\
        Method: BenRoutlette
        Description: Randomly selects an index with weight from fitness scores
        \**************************************************************************/
        public int BenRoutlette(List<Schedule> schedules)
        {
            int totalFitnessScoreWeight = (this.ComputeTotalFitnessScore(schedules)); // maybe add in one ? it avoids divide by zero

            double randomFloat = GA_Controller.GetRandomFloat() * totalFitnessScoreWeight;
            for (int i = 0; i < schedules.Count; i++) // ? current generation will shrink as more and more are moved to next generation 
            {
                randomFloat -= schedules[i].FitnessScore;
                if (randomFloat <= 0)
                {
                    return i;
                }
            }

            return this.schedules.Count - 1; // ? This point should never be reached. roundoff error? ? it could fail on empty schedule
        }

        /**************************************************************************\
        Method: ComputeTotalFitnessScore
        Description: Computes the total fitness score of all schedules
        \**************************************************************************/
        public int ComputeTotalFitnessScore(List<Schedule> schedules)
        {
            int totalFitnessScore = 0;
            foreach (Schedule schedule in schedules)
            {
                totalFitnessScore += schedule.FitnessScore;
            }
            return totalFitnessScore;
        }

        // ? see if moving this to controller results in faster programs
        public void OrderByFitnessScore()
        {
            this.schedules = this.schedules.OrderByDescending(c => c.FitnessScore).ToList();
        }

        public Schedule GetMostFit()
        {
            OrderByFitnessScore();
            return this.Schedules[0];
        }

    }
}
