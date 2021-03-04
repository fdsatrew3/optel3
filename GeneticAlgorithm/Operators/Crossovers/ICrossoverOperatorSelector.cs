using System.Collections.Generic;

namespace GeneticAlgorithm.Operators.Crossovers
{
    public interface ICrossoverOperatorSelector<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Select parents from population to crossover
        /// </summary>
        /// <param name="population">Population for selecting</param>
        /// <returns>Collection of parents</returns>
        IEnumerable<Parents<I>> SelectParents(IPopulation<I> population);
    }
}