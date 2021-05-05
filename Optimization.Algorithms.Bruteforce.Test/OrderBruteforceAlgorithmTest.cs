using System.Linq;
using Xunit;

namespace Optimization.Algorithms.Bruteforce.Test
{
    public class OrderBruteforceAlgorithmTest
    {
        private readonly OrderBruteforceAlgorithm _orderBruteforceAlgorithm;

        public OrderBruteforceAlgorithmTest()
        {
            _orderBruteforceAlgorithm = new OrderBruteforceAlgorithm();
        }

        [Fact]
        public void GetPossibleOrders_ThreeCount_ReturenAllPossibleOrdersResults()
        {
            // Arrange
            var count = 3;
            var expectedOrders = new int[][] 
            { 
                new int[] { 0, 1, 2 }, 
                new int[] { 0, 2, 1 },
                new int[] { 1, 2, 0 },
                new int[] { 1, 0, 2 }, 
                new int[] { 2, 0, 1 }, 
                new int[] { 2, 1, 0 } 
            };

            // Act
            var result = _orderBruteforceAlgorithm.GetPossibleOrders(count).ToArray();

            // Assert
            Assert.Equal(expectedOrders, result);
        }
    }
}
