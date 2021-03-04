using System;
using System.Collections.Generic;
using System.Linq;

using Utilities.Probabilities;

namespace GeneticAlgorithm.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class NonLinearRankingIndividualsSelector<I> : ProbabilityIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        public const double COEFFICIENT_MIN_VALUE = 0;
        public const double COEFFICIENT_MAX_VALUE = 1;

        protected double Coefficient { get; }

        public NonLinearRankingIndividualsSelector(double coefficient, RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
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
            var sortedArray = individualsCollection.OrderBy(x => x.FitnessFunctionValue).ToArray();

            var populationCount = sortedArray.Length;

            for (int i = 0; i < sortedArray.Length; i++)
            {
                var probability = Coefficient * Math.Pow(1 - Coefficient, populationCount - (i + 1));

                yield return new ItemProbability<I>(sortedArray[i], probability);
            }
        }
    }
}
