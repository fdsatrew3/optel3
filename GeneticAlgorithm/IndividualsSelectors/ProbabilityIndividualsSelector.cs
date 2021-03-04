using System;
using System.Collections.Generic;

using Utilities.Probabilities;

namespace GeneticAlgorithm.IndividualsSelectors
{
    public abstract class ProbabilityIndividualsSelector<I> : IndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        protected RandomProbabilityChecker RandomProbabilityChecker { get; }

        protected int SelectionCount { get; }

        public ProbabilityIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
        {
            if (selectionCount < 1)
                throw new ArgumentOutOfRangeException(nameof(selectionCount), "Selection count should be greater than 0");

            RandomProbabilityChecker = randomProbabilityChecker ?? throw new ArgumentNullException(nameof(randomProbabilityChecker), "Random probability checker shouldn't be null");
            SelectionCount = selectionCount;
        }

        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        protected override IEnumerable<I> SelectIndividualsInternal(ICollection<I> individualsCollection)
        {
            for (int i = 0; i < SelectionCount; i++)
            {
                var individualProbabilities = CalculateProbabilities(individualsCollection);
                var item = RandomProbabilityChecker.GetRandomItem(individualProbabilities);

                yield return item;

                individualsCollection.Remove(item);
            }
        }

        /// <summary>
        /// Calculate probabilities for individuals
        /// </summary>
        /// <param name="individuals">Collection of individuals</param>
        /// <returns>Individuals with probabilities</returns>
        protected abstract IEnumerable<ItemProbability<I>> CalculateProbabilities(ICollection<I> individualsCollection);
    }
}
