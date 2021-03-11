using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.Operators.Crossovers;

namespace Optimization.Algorithms.Genetic.Operators.Crossovers
{
    public abstract class CrossoverOperatorBase<I> : ICrossoverOperator<I>
        where I : ICalculatedIndividual
    {
        protected ICrossoverOperatorSelector<I> CrossoverOperatorSelector { get; }

        public CrossoverOperatorBase(ICrossoverOperatorSelector<I> crossoverOperatorSelector)
        {
            CrossoverOperatorSelector = crossoverOperatorSelector;
        }

        /// <summary>
        /// Create childrens from population
        /// </summary>
        /// <param name="population">Current population</param>
        /// <returns>Population of childrens</returns>
        public virtual IEnumerable<I> CreateChildren(IPopulation<I> population)
        {
            foreach(var parents in CrossoverOperatorSelector.SelectParents(population))
            {
                foreach (var child in CreateChildren(parents))
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Create children from parents
        /// </summary>
        /// <param name="parents">Parents</param>
        /// <returns>Children</returns>
        protected abstract IEnumerable<I> CreateChildren(Parents<I> parents);
    }
}
