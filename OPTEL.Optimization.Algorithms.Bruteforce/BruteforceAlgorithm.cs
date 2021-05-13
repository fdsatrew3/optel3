using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;
using Optimization.Algorithms.Core;
using Optimization.Algorithms.Bruteforce;

namespace OPTEL.Optimization.Algorithms.Bruteforce
{
    public class BruteforceAlgorithm<T> : IOptimizationAlgorithm<T>, IOptimizationAlgorithmDecisions<T>
        where T : ProductionPlan, new()
    {
        private readonly IOrderBruteforceAlgorithm _orderBruteforceAlgorithm;
        private readonly ICollection<Order> _orders;
        private readonly ICollection<ProductionLine> _productionLines;
        private readonly IFitnessCalculator<T> _fitnessCalculator;

        public BruteforceAlgorithm(IOrderBruteforceAlgorithm orderBruteforceAlgorithm,
            ICollection<Order> orders,
            ICollection<ProductionLine> productionLines,
            IFitnessCalculator<T> fitnessCalculator)
        {
            _orderBruteforceAlgorithm = orderBruteforceAlgorithm ?? throw new ArgumentNullException(nameof(orderBruteforceAlgorithm));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _productionLines = productionLines ?? throw new ArgumentNullException(nameof(productionLines));
            _fitnessCalculator = fitnessCalculator ?? throw new ArgumentNullException(nameof(fitnessCalculator));
        }

        public IEnumerable<T> GetResolve()
        {
            double? bestFitness = null;
            T bestPlan;

            var productionLinesOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_productionLines.Count).ToArray();
            var ordersOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_orders.Count);

            foreach (var orderOrders in ordersOrders)
            {
                foreach (var productionLinesOrder in productionLinesOrders)
                {
                    var plan = MakeProductionLineQueue(productionLinesOrder, orderOrders);
                    var testFitness = _fitnessCalculator.Calculate(plan);

                    if (bestFitness is null || testFitness > bestFitness)
                    {
                        bestPlan = plan;
                        bestFitness = testFitness;

                        yield return bestPlan;
                    }
                }
            }
        }

        T IOptimizationAlgorithm<T>.GetResolve()
        {
            double? bestFitness = null;
            T bestPlan = null;

            var productionLinesOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_productionLines.Count).ToArray();
            var ordersOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_orders.Count);

            foreach (var orderOrders in ordersOrders)
            {
                foreach (var productionLinesOrder in productionLinesOrders)
                {
                    var plan = MakeProductionLineQueue(productionLinesOrder, orderOrders);
                    var testFitness = _fitnessCalculator.Calculate(plan);

                    if (bestFitness is null || testFitness > bestFitness)
                    {
                        bestPlan = plan;
                        bestFitness = testFitness;
                    }
                    
                }
            }

            return bestPlan;
        }

        private T MakeProductionLineQueue(int[] productionLinesOrders, int[] ordersOrders)
        {
            var result = new T { ProductionLineQueues = new List<ProductionLineQueue>() };

            foreach(var order in productionLinesOrders)
            {
                result.ProductionLineQueues.Add(new ProductionLineQueue { Orders = new List<Order>(), ProductionLine = _productionLines.ElementAt(order) });
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
