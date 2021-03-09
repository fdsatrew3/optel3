using System;
using System.Collections.Generic;
using System.Linq;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.Base
{
    public class BestSelector<T> : IBestSelector<T>
        where T : ICalculatedIndividual
    {
        /// <summary>
        /// Return best individual form population
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>Best individual</returns>
        public T SelectBestIndividual(IPopulation<T> currentPopulation)
        {
            return SelectBestIndividual(currentPopulation.Individuals);
        }

        /// <summary>
        /// Return best individual form individuals collection
        /// </summary>
        /// <param name="individuals">Individuals collection </param>
        /// <returns>Best individual</returns>
        public T SelectBestIndividual(IEnumerable<T> individuals)
        {
            if (individuals is null)
                throw new ArgumentNullException(nameof(individuals), "Enumerable of Individual is null");

            return individuals.OrderByDescending(x => x.FitnessFunctionValue).FirstOrDefault();
        }
    }
}
