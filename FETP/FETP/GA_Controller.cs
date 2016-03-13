﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    struct Exam
    {
        TimeQuantum duration;
    }

    struct TimeSlot
    {
    }

    struct TimeQuantum
    {
    }

    /**************************************************************************\
    Class: GA_Controller (Genetic Algorithm Controller)
    Description: Includes all of the primary functions needed for the genetic
                 algorithm and evolving the chromosomes to find a solution.
    \**************************************************************************/ 
    class GA_Controller
    {
        /**************************************************************************\
        GA_Controller - Data Members
        \**************************************************************************/ 
        private int crossoverRate;
        private int maxGenerations;
        private int mutationRate;
        private int populationSize;

        private Exam[] setExams;
        private int timeQuantumLength;
        private TimeQuantum[] setTimeQuanta;
        private TimeQuantum[] setSuitableTimeQuanta;


        /**************************************************************************\
        GA_Controller - Methods 
        \**************************************************************************/ 
        /**************************************************************************\
        Constructor: Default 
        Description: 
        \**************************************************************************/ 
        GA_Controller()
        {
        }


        /**************************************************************************\
        Method: Assign Fitness 
        Description: Should take a chromosome as an input and output its fitness
                     score.
        \**************************************************************************/ 
        float AssignFitness()
        {
            return 0.0f;
        }


        /**************************************************************************\
        Method: Crossover 
        Description: Takes two chromosomes as an input and performs crossover on
                     the genes to create two offspring based on the crossover rate.
        \**************************************************************************/ 
        void Crossover()
        {
        }


        /**************************************************************************\
        Method: Roulette 
        Description: Selects a chromosome from the population proportional to its
                     fitness score. This is used to select members from the
                     population to go on to the next generation. It does not
                     guarantee that the most fit chromosomes will be selected, but
                     it does give them a good chance of doing so.
        \**************************************************************************/ 
        void Roulette()
        {
        }


        /**************************************************************************\
        Method: Mutate 
        Description: Creates a mutation in a chromosome depending on the mutation
                     rate.
        \**************************************************************************/ 
        void Mutate()
        {
        }
    }
}
