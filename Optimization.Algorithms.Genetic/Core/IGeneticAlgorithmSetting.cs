using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IGeneticAlgorithmSetting<I>        
        where I : class, ICalculatedIndividual
    {
        int MaxPopulationCount { get; }
        
        IStartPopulationCreator<I> StartPopulationCreator { get; }

        IMutationOperator<I> MutationOperator { get; }

        ICrossoverOperator<I> CrossoverOperator { get; }

        IPopulationSelector<I> PopulationSelector { get; }

        IEnumerable<IFinalCoditionChecker<I>> FinalCoditionCheckers { get; }

        IBestSelector<I> BestSelector { get; }
    }
}
