using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;
using Optimization.Algorithms;
using System;
using System.Linq;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class CostFunctionCalculator<T> : ITargetFunctionCalculator<T>
        where T : ProductionPlan
    {
        public IProductionLineQueueCostCalculator ProductionLineQueueCostCalculator { get; }

        public CostFunctionCalculator(IProductionLineQueueCostCalculator productionLineQueueCostCalculator)
        {
            ProductionLineQueueCostCalculator = productionLineQueueCostCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueCostCalculator));
        }

        public double Calculate(T individual) => individual.ProductionLineQueues.Select(x => ProductionLineQueueCostCalculator.Calculate(x)).Max();
    }
}
