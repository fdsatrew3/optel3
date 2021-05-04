using System.Collections.Generic;

namespace Optimization.Algorithms.Bruteforce
{
    public interface IOrderBruteforceAlgorithm
    {
        /// <summary>
        /// Generate all possible orders
        /// </summary>
        /// <param name="count">Count of positions</param>
        /// <returns>Possible orders</returns>
        IEnumerable<int[]> GetPossibleOrders(int count);
    }
}
