using OPTEL.Data;
using OPTEL.Optimization.Algorithms.Bruteforce.Core;
using OPTEL.Optimization.Algorithms.Bruteforce.Data;
using Optimization.Algorithms.Utilities.Extensions;
using Optimization.Algorithms.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OPTEL.Optimization.Algorithms.Bruteforce
{
    public class BruteforceAlgorithm : IOptimizationAlgorithm<ProductionPlan>, IOptimizationAlgorithmDecisions<ProductionPlan>
    {
        private readonly ICollection<Order> _orders;
        private readonly ICollection<ProductionLine> _productionLines;
        private readonly IFitnessFunctionCalculator<ProductionPlan> _fitnessFunctionCalculator;

        public BruteforceAlgorithm(ICollection<Order> orders, ICollection<ProductionLine> productionLines, IFitnessFunctionCalculator<ProductionPlan> fitnessFunctionCalculator)
        {
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _productionLines = productionLines ?? throw new ArgumentNullException(nameof(productionLines));
            _fitnessFunctionCalculator = fitnessFunctionCalculator ?? throw new ArgumentNullException(nameof(fitnessFunctionCalculator));
        }

        public IEnumerable<ProductionPlan> GetResolve()
        {
            double? bestFitness = null;
            ProductionPlan bestPlan = null;

            var productionLinesOrders = BruteforceOrder(_productionLines.Count).ToArray();

            foreach (var ordersOrders in BruteforceOrder(_orders.Count))
            {
                foreach (var productionLinesOrder in productionLinesOrders)
                {
                    var plan = MakeProductionLineQueue(productionLinesOrder, ordersOrders);
                    var testFitness = _fitnessFunctionCalculator.Calculate(plan);

                    if (bestFitness is null || testFitness > bestFitness)
                    {
                        bestPlan = plan;
                        bestFitness = testFitness;

                        yield return bestPlan;
                    }
                }
            }
        }

        ProductionPlan IOptimizationAlgorithm<ProductionPlan>.GetResolve()
        {
            double? bestFitness = null;
            ProductionPlan bestPlan = null;

            var productionLinesOrders = BruteforceOrder(_productionLines.Count).ToArray();

            foreach (var ordersOrders in BruteforceOrder(_orders.Count))
            {
                foreach (var productionLinesOrder in productionLinesOrders)
                {
                    var plan = MakeProductionLineQueue(productionLinesOrder, ordersOrders);
                    var testFitness = _fitnessFunctionCalculator.Calculate(plan);

                    if (bestFitness is null || testFitness > bestFitness)
                    {
                        bestPlan = plan;
                        bestFitness = testFitness;
                    }
                    
                }
            }

            return bestPlan;
        }

        public static IEnumerable<int[]> BruteforceOrder(int count)
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

        private ProductionPlan MakeProductionLineQueue(int[] productionLinesOrders, int[] ordersOrders)
        {
            var result = new ProductionPlan { ProductionLineQueues = new List<ProductionLineQueue>() };

            foreach(var order in productionLinesOrders)
            {
                result.ProductionLineQueues.Add(new ProductionLineQueue { Orders = new List<Order>(), ProductionLine = _productionLines.ElementAt(order)});
            }

            int i = 0;

            foreach(var order in ordersOrders)
            {
                if (i == productionLinesOrders.Length)
                    i = 0;

                result.ProductionLineQueues.ElementAt(i).Orders.Add(_orders.ElementAt(order));

                i++;
            }

            return result;
        }
    }
}
