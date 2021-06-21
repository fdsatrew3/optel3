using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface ICrossoverOperator<I>
        
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Create childrens from population
        /// </summary>
        /// <param name="population">Current population</param>
        /// <returns>Population of childrens</returns>
        IEnumerable<I> CreateChildren(IPopulation<I> population);
    }
}
