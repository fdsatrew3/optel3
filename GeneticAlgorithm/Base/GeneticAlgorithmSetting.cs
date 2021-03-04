﻿using System.Collections.Generic;

namespace GeneticAlgorithm.Base
{
    public class GeneticAlgorithmSetting<I> : IGeneticAlgorithmSetting<I>        
        where I : class, ICalculatedIndividual
    {
        public int MaxPopulationCount { get; set; }

        public IStartPopulationCreator<I> StartPopulationCreator { get; set; }
         
        public IMutationOperator<I> MutationOperator { get; set; }
         
        public ICrossoverOperator<I> CrossoverOperator { get; set; }
         
        public IPopulationSelector<I> PopulationSelector { get; set; }

        public IEnumerable<IFinalCoditionChecker<I>> FinalCoditionCheckers { get; set; }

        public IBestSelector<I> BestSelector { get; set; }

        public GeneticAlgorithmSetting()
        {
        
        }
    }
}
