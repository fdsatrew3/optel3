using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Utilities.Extensions;

namespace Optimization.Algorithms.Bruteforce
{
    public class OrderBruteforceAlgorithm : IOrderBruteforceAlgorithm
    {
        public IEnumerable<int[]> GetPossibleOrders(int count)
        {
            return GetPossibleOrdersInternal(count);
        }

        private static IEnumerable<int[]> GetPossibleOrdersInternal(int count)
        {
            var orderVariation = new int[count];

            for (int i = 0; i < orderVariation.Length; i++)
            {
                orderVariation[i] = i;
            }

            var counts = new int[orderVariation.Length];

            int j = orderVariation.Length - 2;

            yield return orderVariation.ToArray();

            while (j >= 0)
            {
                if (counts.Length - 1 - j > counts[j])
                {
                    counts[j]++;
                    orderVariation.Swap(j, orderVariation.Length - 1);
                    yield return orderVariation.ToArray();
                    j = orderVariation.Length - 2;
                }
                else
                {
                    counts[j] = 0;
                    j--;
                }
            }
        }
    }
}
