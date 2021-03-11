using System;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.Base
{
    public class PopulationSelector<I> : IPopulationSelector<I>
        where I : ICalculatedIndividual
    {
        protected IIndividualsSelector<I> IndividualsSelector { get; }

        public PopulationSelector(IIndividualsSelector<I> individualsSelector)
        {
            IndividualsSelector = individualsSelector ?? throw new ArgumentNullException(nameof(individualsSelector), "Individual selctor should not be null");
        }

        /// <summary>
        /// Select new population from current population
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>Summary population</returns>
        public virtual IPopulation<I> SelectPopulation(IPopulation<I> currentPopulation)
        {
            if (currentPopulation.Individuals.Count < 2)
                throw new ArgumentOutOfRangeException("Current population individuals count should be grater than or equal to 2", nameof(currentPopulation.Individuals));

            return new Population<I>(IndividualsSelector.SelectIndividuals(currentPopulation.Individuals));
        }
    }
}
