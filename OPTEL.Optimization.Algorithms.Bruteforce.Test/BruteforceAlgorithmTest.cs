using System.Linq;
using Xunit;

namespace OPTEL.Optimization.Algorithms.Bruteforce.Test
{
    public class BruteforceAlgorithmTest
    {
        [Fact]
        public void fun()
        {
            var e = new BruteforceAlgorithm<object>();

            var e1 = e.BruteforceOrder(3).ToArray();
            var e2 = e.BruteforceOrder(4).ToArray();
        }
    }
}
