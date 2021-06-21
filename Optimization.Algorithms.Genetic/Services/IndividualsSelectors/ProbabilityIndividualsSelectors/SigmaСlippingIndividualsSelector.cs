using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Utilities.Probabilities;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class SigmaСlippingIndividualsSelector<I> : ProbabilityIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        public SigmaСlippingIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
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
            var averageTargetFunctionValue = individualsCollection.Average(x => x.TargetFunctionValue);
            var dispertion = CalculateDispersion(individualsCollection, averageTargetFunctionValue);

            var sumScalingObjectiveFunctionValue = individualsCollection.Sum(x => CalculateScalingObjectiveFunctionValue(x, dispertion, averageTargetFunctionValue));

            foreach (var individual in individualsCollection)
            {
                var probability = CalculateScalingObjectiveFunctionValue(individual, dispertion, averageTargetFunctionValue) / sumScalingObjectiveFunctionValue;

                yield return new ItemProbability<I>(individual, probability);
            }
        }

        protected double CalculateDispersion(IEnumerable<I> individuals, double averageTargetFunctionValue)
        {
            var sumDifferencesWithMean = individuals.Sum(x => Math.Pow(x.TargetFunctionValue - averageTargetFunctionValue, 2));

            return sumDifferencesWithMean / individuals.Count();
        }

        protected double CalculateScalingObjectiveFunctionValue(I individual, double dispertion, double averageTargetFunctionValue)
        {
            return 1 + ((individual.TargetFunctionValue - averageTargetFunctionValue) / (2 * Math.Sqrt(dispertion)));
        }
    }
}
