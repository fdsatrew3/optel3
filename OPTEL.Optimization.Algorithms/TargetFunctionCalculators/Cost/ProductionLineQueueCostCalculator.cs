using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using System;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class ProductionLineQueueCostCalculator : IProductionLineQueueCostCalculator
    {
        public IProductionLineQueueTimeCalculator TimeCalculator { get; }

        public ProductionLineQueueCostCalculator(IProductionLineQueueTimeCalculator timeCalculator)
        {
            TimeCalculator = timeCalculator ?? throw new ArgumentNullException(nameof(timeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
            => (TimeCalculator.Calculate(productionLineQueue) / 3600) * Convert.ToDouble(productionLineQueue.ProductionLine.HourCost);
    }
}
