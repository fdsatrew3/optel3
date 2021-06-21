using System;
using System.Linq;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class ReconfigurationCostCalculator : IReconfigurationCostCalculator
    {
        private readonly IOrdersReconfigurationCostCalculator _ordersReconfigurationCostCalculator;

        public ReconfigurationCostCalculator(IOrdersReconfigurationCostCalculator ordersReconfigurationCostCalculator)
        {
            _ordersReconfigurationCostCalculator = ordersReconfigurationCostCalculator ?? throw new ArgumentNullException(nameof(ordersReconfigurationCostCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
        {
            var result = 0d;

            for (int i = 0; i < productionLineQueue.Orders.Count - 1; i++)
            {
                result += _ordersReconfigurationCostCalculator.Calculate(productionLineQueue.ProductionLine,
                    productionLineQueue.Orders.ElementAt(i),
                    productionLineQueue.Orders.ElementAt(i + 1));
            }

            return result;
        }
    }
}
