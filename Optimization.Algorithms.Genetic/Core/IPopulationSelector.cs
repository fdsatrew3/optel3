using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IPopulationSelector<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Select new population from current population
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>Summary population</returns>
        IPopulation<I> SelectPopulation(IPopulation<I> currentPopulation);
    }
}
