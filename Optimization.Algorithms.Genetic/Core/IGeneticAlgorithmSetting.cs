using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IGeneticAlgorithmSetting<I>        
        where I : class, ICalculatedIndividual
    {
        int MaxPopulationCount { get; }
        
        IStartPopulationGenerator<I> StartPopulationCreator { get; }

        IMutationOperator<I> MutationOperator { get; }

        ICrossoverOperator<I> CrossoverOperator { get; }

        IPopulationSelector<I> PopulationSelector { get; }

        IEnumerable<IFinalConditionChecker<IPopulation<I>>> FinalCoditionCheckers { get; }

        IBestSelector<I> BestSelector { get; }
    }
}
