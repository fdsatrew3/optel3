using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;
using Optimization.Algorithms;
using Optimization.Algorithms.Core;
using Optimization.Algorithms.Bruteforce;
using Optimization.Algorithms.Utilities.Extensions;

namespace OPTEL.Optimization.Algorithms.Bruteforce
{
    public class BruteforceAlgorithm<T> : IOptimizationAlgorithm<T>, IOptimizationAlgorithmDecisions<T>
        where T : ProductionPlan, new()
    {
        private readonly IOrderBruteforceAlgorithm _orderBruteforceAlgorithm;
        private readonly ICollection<Order> _orders;
        private readonly ICollection<ProductionLine> _productionLines;
        private readonly IFitnessCalculator<T> _fitnessCalculator;
        private readonly IEnumerable<IFinalConditionChecker<T>> _finalConditionCheckers;

        public BruteforceAlgorithm(IOrderBruteforceAlgorithm orderBruteforceAlgorithm,
            ICollection<Order> orders,
            ICollection<ProductionLine> productionLines,
            IFitnessCalculator<T> fitnessCalculator,
            IEnumerable<IFinalConditionChecker<T>> finalConditionCheckers)
        {
            _orderBruteforceAlgorithm = orderBruteforceAlgorithm ?? throw new ArgumentNullException(nameof(orderBruteforceAlgorithm));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _productionLines = productionLines ?? throw new ArgumentNullException(nameof(productionLines));
            _fitnessCalculator = fitnessCalculator ?? throw new ArgumentNullException(nameof(fitnessCalculator));
            _finalConditionCheckers = finalConditionCheckers;
        }

        public IEnumerable<T> GetResolve()
        {
            double? bestFitness = null;
            T bestPlan;

            foreach (var resolve in GetResolvesInternal())
            {
                var testFitness = _fitnessCalculator.Calculate(resolve);

                if (bestFitness is null || testFitness > bestFitness)
                {
                    bestPlan = resolve;
                    bestFitness = testFitness;

                    yield return resolve;
                }
            }
        }

        T IOptimizationAlgorithm<T>.GetResolve()
        {
            double? bestFitness = null;
            T bestPlan = null;

            foreach (var resolve in GetResolvesInternal())
            {
                var testFitness = _fitnessCalculator.Calculate(resolve);

                if (bestFitness is null || testFitness > bestFitness)
                {
                    bestPlan = resolve;
                    bestFitness = testFitness;
                }
            }

            return bestPlan;
        }

        private IEnumerable<T> GetResolvesInternal()
        {
            if (_finalConditionCheckers != null)
                _finalConditionCheckers.ForEach(x => x.Begin());

            var productionLinesOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_productionLines.Count).ToArray();
            var ordersOrders = _orderBruteforceAlgorithm.GetPossibleOrders(_orders.Count);

            foreach (var orderOrders in ordersOrders)
            {
                foreach (var productionLinesOrder in productionLinesOrders)
                {
                    var resolve = MakeProductionLineQueue(productionLinesOrder, orderOrders);

                    if (_finalConditionCheckers != null && _finalConditionCheckers.Any(x => x.IsStateFinal(resolve)))
                        yield break;
                    else
                        yield return resolve;
                }
            }
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
