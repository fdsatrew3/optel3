using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;
using Optimization.Algorithms;
using System;
using System.Linq;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class CostFunctionCalculator : ITargetFunctionCalculator<ProductionPlan>
    {
        public IProductionLineQueueCostCalculator ProductionLineQueueCostCalculator { get; }

        public CostFunctionCalculator(IProductionLineQueueCostCalculator productionLineQueueCostCalculator)
        {
            ProductionLineQueueCostCalculator = productionLineQueueCostCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueCostCalculator));
        }

        public double Calculate(ProductionPlan individual) => individual.ProductionLineQueues.Select(x => ProductionLineQueueCostCalculator.Calculate(x)).Max();
    }
}
