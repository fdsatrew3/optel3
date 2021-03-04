using System.Collections.Generic;

namespace GeneticAlgorithm
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
