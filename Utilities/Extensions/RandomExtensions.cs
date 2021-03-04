using System;

namespace Utilities.Extensions
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
    }
}
