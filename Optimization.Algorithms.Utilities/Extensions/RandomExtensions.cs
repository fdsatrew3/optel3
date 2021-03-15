using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.Algorithms.Utilities.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Return random double from minimum and maximum
        /// </summary>
        /// <param name="random">Extension parameter</param>
        /// <param name="minimum">Min border</param>
        /// <param name="maximum">Max border</param>
        /// <returns>Random double</returns>
        public static double NextDouble(this Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Return random boolean value
        /// </summary>
        /// <param name="random">Extension parameter</param>
        /// <returns>Random boolean</returns>
        public static bool NextBoolean(this Random random)
        {
            return random.Next(0, 2) == 1;
        }

        /// <summary>
        /// Check probability (return true if random value go through probability)
        /// </summary>
        /// <param name="random">Extension parameter</param>
        /// <param name="probability">Probability to check (from 0 to 1)</param>
        /// <returns>Result of checking probability</returns>
        public static bool CheckProbability(this Random random, double probability)
        {
            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be between 0 and 1");

            return random.NextDouble() < probability;
        }

        /// <summary>
        /// Choose random element of collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="random">Extension parameter</param>
        /// <param name="collection">Collection to choose element</param>
        /// <returns>Random element of collection</returns>
        public static T NextElement<T>(this Random random, ICollection<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), "Collection should not be null");
            else if (collection.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(collection.Count), "Collection count should be greater than 0");

            return collection.ElementAt(random.Next(collection.Count));
        }

        /// <summary>
        /// Choose random elements of collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="random">Extension parameter</param>
        /// <param name="collection">Collection to choose elements</param>
        /// <param name="count">Count of elements to choose</param>
        /// <returns>Pointed count of collection's elements in random order</returns>
        public static IEnumerable<T> NextElements<T>(this Random random, ICollection<T> collection, int count)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), "Collection should not be null");
            else if (collection.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(collection.Count), "Collection count should be greater than 0");
            else if (count <= 0 || count > collection.Count)
                throw new ArgumentOutOfRangeException(nameof(count), "Elements count should be positive number lesser or equal to collection count");

            var newCollection = collection.ToList();

            for (int i = 0; i < count; i++)
            {
                var randomElement = random.NextElement(newCollection);
                yield return randomElement;

                newCollection.Remove(randomElement);
            }
        }

        /// <summary>
        /// Choose random elements of collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="random">Extension parameter</param>
        /// <param name="collection"></param>
        /// <returns>Elements of collection in random order</returns>
        public static IEnumerable<T> NextElements<T>(this Random random, ICollection<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), "Collection should not be null");
            else if (collection.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(collection.Count), "Collection count should be greater than 0");

            return random.NextElements(collection, collection.Count);
        }
    }
}
