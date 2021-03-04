using System;
using System.Collections.Generic;
using System.Linq;

using Optimization.Algorithms.Utilities.Probabilities;

namespace GeneticAlgorithm.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class RouletteIndividualsSelector<I> : ProbabilityIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        public RouletteIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
            : base(randomProbabilityChecker, selectionCount)
        {

        }

        /// <summary>
        /// Calculate probabilities for individuals
        /// </summary>
        /// <param name="individuals">Collection of individuals</param>
        /// <returns>Individuals with probabilities</returns>
        protected override IEnumerable<ItemProbability<I>> CalculateProbabilities(ICollection<I> individualsCollection)
        {
            var sumFitness = individualsCollection.Sum(x => x.FitnessFunctionValue);

            foreach (var individual in individualsCollection)
            {
                var probability = individual.FitnessFunctionValue / sumFitness;

                yield return new ItemProbability<I>(individual, probability);
            }
        }
    }
}
