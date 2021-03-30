using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Operators.Crossovers;
using Optimization.Algorithms.Genetic.Services.Operators.Crossovers;
using Optimization.Algorithms.Utilities.Extensions;

using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Operators.Crossovers
{
    public class PointedCrossoverOperator : CrossoverOperatorBase<ProductionPlan>
    {
        private readonly double _probability;
        private readonly Random _random;

        public PointedCrossoverOperator(ICrossoverOperatorSelector<ProductionPlan> crossoverOperatorSelector, double probability, Random random) : base(crossoverOperatorSelector)
        {
            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be in range between 0 and 1.");

            _probability = probability;
            _random = random ?? throw new ArgumentNullException(nameof(random), "Random should be in range between 0 and 1.");
        }

        protected override IEnumerable<ProductionPlan> CreateChildren(Parents<ProductionPlan> parents)
        {
            yield return CreateChild(parents.FirstParent, parents.SecondParent);
            yield return CreateChild(parents.SecondParent, parents.FirstParent);
        }

        private ProductionPlan CreateChild(ProductionPlan firstParent, ProductionPlan secondParent)
        {
            var children = firstParent.Clone() as ProductionPlan;

            foreach(var order in SelectOrdersToCrossover(secondParent))
            {
                ReplaceOrder(firstParent, secondParent, order);
            }

            return children;
        }

        private IEnumerable<Order> SelectOrdersToCrossover(ProductionPlan productionPlan)
        {
            var productionLineQueues = productionPlan.ProductionLineQueues.ToList();

            foreach (var productionLineQueue in productionLineQueues)
            {
                var orders = productionLineQueue.Orders.ToList();

                foreach (var order in orders)
                {
                    if (_random.CheckProbability(_probability))
                        yield return order;
                }
            }
        }

        private void ReplaceOrder(ProductionPlan children, ProductionPlan secondParent, Order order)
        {
            foreach(var queue in secondParent.ProductionLineQueues)
            {
                var index = queue.Orders.IndexOf(order);

                if (index >= 0)
                {
                    RemoveOrderFromPlan(children, order);

                    var targetQueue = children.ProductionLineQueues.First(x => x.ProductionLine == queue.ProductionLine);

                    if (targetQueue.Orders.Count > index)
                        targetQueue.Orders.Add(order);
                    else
                        targetQueue.Orders.Insert(index, order);
                }
            }
        }

        private void RemoveOrderFromPlan(ProductionPlan productionPlan, Order order)
        {
            foreach (var queue in productionPlan.ProductionLineQueues)
            {
                if (queue.Orders.Contains(order))
                {
                    queue.Orders.Remove(order);
                    break;
                }
            }
        }
    }
}
