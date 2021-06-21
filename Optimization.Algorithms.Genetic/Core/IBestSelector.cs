using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IBestSelector<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Return best individual form population
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>Best individual</returns>
        I SelectBestIndividual(IPopulation<I> currentPopulation);

        /// <summary>
        /// Return best individual form individuals collection
        /// </summary>
        /// <param name="individuals">Individuals collection </param>
        /// <returns>Best individual</returns>
        I SelectBestIndividual(IEnumerable<I> individuals);
    }
}
