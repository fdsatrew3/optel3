using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.Base
{
    public class GeneticAlgorithmSetting<I> : IGeneticAlgorithmSetting<I>        
        where I : class, ICalculatedIndividual
    {
        public int MaxPopulationCount { get; set; }

        public IStartPopulationGenerator<I> StartPopulationCreator { get; set; }
         
        public IMutationOperator<I> MutationOperator { get; set; }
         
        public ICrossoverOperator<I> CrossoverOperator { get; set; }
         
        public IPopulationSelector<I> PopulationSelector { get; set; }

        public IEnumerable<IFinalCoditionChecker<IPopulation<I>>> FinalCoditionCheckers { get; set; }

        public IBestSelector<I> BestSelector { get; set; }

        public GeneticAlgorithmSetting()
        {
        
        }
    }
}
