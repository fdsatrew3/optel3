using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Operators.Crossovers.Selectors
{
    public abstract class CrossoverOperatorSelector<I> : ICrossoverOperatorSelector<I>
        where I : ICalculatedIndividual
    {
        protected IIndividualsSelector<I> IndividualsSelector { get; }

        public CrossoverOperatorSelector(IIndividualsSelector<I> individualsSelector)
        {
            IndividualsSelector = individualsSelector ?? throw new ArgumentNullException(nameof(individualsSelector), "Individuals selector should not be null");
        }

        /// <summary>
        /// Select parents from population to crossover
        /// </summary>
        /// <param name="population">Population for selecting</param>
        /// <returns>Collection of parents</returns>
        public IEnumerable<Parents<I>> SelectParents(IPopulation<I> population)
        {
            var parentsPull = IndividualsSelector.SelectIndividuals(population.Individuals).ToArray();

            foreach(var individual in parentsPull)
            {
                var secondParent = SelectSecondParent(individual, parentsPull);

                yield return new Parents<I> { FirstParent = individual, SecondParent = secondParent };
            }
        }

        /// <summary>
        /// Find second parent to individual from parents pull
        /// </summary>
        /// <param name="firstParent">First parent to find pair</param>
        /// <param name="parentsPull">All individuals pull</param>
        /// <returns>Second parent</returns>
        protected abstract I SelectSecondParent(I firstParent, ICollection<I> parentsPull);
    }
}
