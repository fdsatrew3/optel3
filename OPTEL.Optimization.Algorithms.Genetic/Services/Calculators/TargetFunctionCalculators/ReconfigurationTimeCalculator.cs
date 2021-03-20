using System;
using System.Linq;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators
{
    public class ReconfigurationTimeCalculator : IReconfigurationTimeCalculator
    {
        public IOrdersReconfigurationTimeCalculator OrdersReconfigurationTimeCalculator { get; }

        public ReconfigurationTimeCalculator(IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator)
        {
            OrdersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator ?? throw new ArgumentNullException(nameof(ordersReconfigurationTimeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
        {
            var result = 0d;

            for (int i = 0; i < productionLineQueue.Orders.Count - 1; i++)
            {
                result += OrdersReconfigurationTimeCalculator.Calculate(productionLineQueue.Extruder,
                    productionLineQueue.Orders.ElementAt(i),
                    productionLineQueue.Orders.ElementAt(i + 1));
            }

            return result;
        }
    }
}
