using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Probabilities
{
    public class RandomProbabilityChecker
    {
        protected Random Random { get; }

        public RandomProbabilityChecker(Random random)
        {
            Random = random;
        }

        /// <summary>
        /// Generate one element from collection of elements probabilities which sum is equal to 1
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="itemProbabilities">Collection of elements probabilities which sum is equal to 1</param>
        /// <returns>Random element</returns>
        public T GetRandomItem<T>(IEnumerable<ItemProbability<T>> itemProbabilities)
        {
            var randomValue = Random.NextDouble();

            double probabilitySum = 0;

            foreach(var item in itemProbabilities)
            {
                probabilitySum += item.Probability;

                if (probabilitySum >= randomValue)
                    return item.Item;
            }

            return itemProbabilities.Last().Item;
        }

        /// <summary>
        /// Generate one element from collection of elements probabilities which sum is equal to 1
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="itemProbabilities">Collection of elements probabilities which sum is equal to 1</param>
        /// <returns>Random element</returns>
        public T CheckProbability<T>(IEnumerable<ItemProbability<T>> itemProbabilities)
        {
            if (itemProbabilities.Sum(x => x.Probability) != 1)
                throw new ArgumentException("Sum of item probabilities should be equal to 1", nameof(itemProbabilities));

            var randomValue = Random.NextDouble();

            double probabilitySum = 0;

            foreach(var item in itemProbabilities)
            {
                probabilitySum += item.Probability;

                if (probabilitySum >= item.Probability)
                    return item.Item;
            }

            throw new InvalidOperationException("Checker can't choose one of items. Check input collection");
        }
    }
}
