using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Services.Operators.Mutations;
using Optimization.Algorithms.Utilities.Extensions;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using Order = OPTEL.Data.Order;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Operators.Mutations
{
    public class PointedMutationOperator : MutationOperatorBase<ProductionPlan>
    {
        private readonly Random _random;
        private readonly double _probability;

        public PointedMutationOperator(IIndividualsSelector<ProductionPlan> individualsSelector, Random random, double probability) : base(individualsSelector)
        {
            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be in range between 0 and 1.");

            _probability = probability;
            _random = random ?? throw new ArgumentNullException(nameof(random), "Random should be in range between 0 and 1.");
        }

        protected override void MakeMutation(ProductionPlan individual)
        {
            foreach(var order in SelectOrdersToMutate(individual))
            {
                MutateOrderPosition(individual, order);
            }
        }

        private IEnumerable<Order> SelectOrdersToMutate(ProductionPlan productionPlan)
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

        private void MutateOrderPosition(ProductionPlan productionPlan, Order order)
        {
            var ordersCount = productionPlan.ProductionLineQueues.Sum(x => x.Orders.Count);
            var newIndex = _random.Next(ordersCount);

            int currentOrdersCount = 0;

            foreach (var productionLineQueue in productionPlan.ProductionLineQueues)
            {
                if (currentOrdersCount + productionLineQueue.Orders.Count > newIndex)
                    currentOrdersCount += productionLineQueue.Orders.Count;
                else
                    productionLineQueue.Orders.Insert(newIndex - currentOrdersCount, order);
            }
        }
    }
}
