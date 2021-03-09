using System;
using System.Collections.Generic;
using System.Linq;

using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.IndividualsSelectors
{
    public class UniformRankingIndividualsSelector<I> : IndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        protected Random Random { get; }

        protected int SelectionCount { get; }

        protected int MaxSelectionCount { get; }

        public UniformRankingIndividualsSelector(Random random, int selectionCount, int maxSelectionCount = 0)
        {
            if (selectionCount < 1)
                throw new ArgumentOutOfRangeException(nameof(selectionCount), "Selection count should be greater than 0");

            if (maxSelectionCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxSelectionCount), "Max selection count should be greater than or equal to 0");
            else if (maxSelectionCount != 0 && maxSelectionCount < selectionCount)
                throw new ArgumentOutOfRangeException(nameof(maxSelectionCount), "Max selection count should be greater than or equal to selection count");

            Random = random ?? throw new ArgumentNullException(nameof(random), "Random object shouldn't be null");
            SelectionCount = selectionCount;
            MaxSelectionCount = maxSelectionCount;
        }

        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        protected override IEnumerable<I> SelectIndividualsInternal(ICollection<I> individualsCollection)
        {
            if (individualsCollection.Count <= 0)
                throw new ArgumentException("Current population individuals count should be grater than 0", nameof(individualsCollection));
            else if (MaxSelectionCount != 0 && MaxSelectionCount > individualsCollection.Count)
                throw new ArgumentException("Current population individuals count should be lesser than or equal to max selection count, if it is different to 0", nameof(individualsCollection));

            return SelectRandomIndividuals(individualsCollection);
        }

        private IEnumerable<I> SelectRandomIndividuals(ICollection<I> individualsCollection)
        {
            var maxSelectionCount = MaxSelectionCount == 0 ? individualsCollection.Count : MaxSelectionCount;

            for (int i = 0; i < SelectionCount; i++)
            {
                int index = Random.Next(0, maxSelectionCount);
                var item = individualsCollection.ElementAt(index);

                yield return item;

                individualsCollection.Remove(item);
                maxSelectionCount--;
            }
        }
    }
}
