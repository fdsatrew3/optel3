using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Utilities.Probabilities;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class LinearRankingIndividualsSelector<I> : ProbabilityIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        public const double COEFFICIENT_MIN_VALUE = 1;
        public const double COEFFICIENT_MAX_VALUE = 2;

        protected double Coefficient { get; }

        public LinearRankingIndividualsSelector(double coefficient, RandomProbabilityChecker randomProbabilityChecker, int selectionCount) 
            : base(randomProbabilityChecker, selectionCount)
        {
            if (coefficient < COEFFICIENT_MIN_VALUE || coefficient > COEFFICIENT_MAX_VALUE)
                throw new ArgumentOutOfRangeException(nameof(coefficient), $"Coefficient should be between {COEFFICIENT_MIN_VALUE} and {COEFFICIENT_MAX_VALUE}");
            Coefficient = coefficient;
        }

        /// <summary>
        /// Calculate probabilities for individuals
        /// </summary>
        /// <param name="individuals">Collection of individuals</param>
        /// <returns>Individuals with probabilities</returns>
        protected override IEnumerable<ItemProbability<I>> CalculateProbabilities(ICollection<I> individualsCollection)
        {
            var sortedArray = individualsCollection.OrderByDescending(x => x.FitnessFunctionValue).ToList();

            var populationCount = (double)sortedArray.Count;

            for (int i = 0; i < sortedArray.Count; i++)
            {
                var probability = (1 / populationCount) * (Coefficient - ((2 * Coefficient - 2) * (i / (populationCount - 1))));

                yield return new ItemProbability<I>(sortedArray[i], probability);
            }
        }
    }
}
