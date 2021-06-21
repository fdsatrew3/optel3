using System;
using System.Linq;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class CostFunctionCalculator<T> : ITargetFunctionCalculator<T>
        where T : ProductionPlan
    {
        private readonly IProductionLineQueueCostCalculator _productionLineQueueCostCalculator;

        public CostFunctionCalculator(IProductionLineQueueCostCalculator productionLineQueueCostCalculator)
        {
            _productionLineQueueCostCalculator = productionLineQueueCostCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueCostCalculator));
        }

        public double Calculate(T individual) => 
            individual.ProductionLineQueues.Select(x => _productionLineQueueCostCalculator.Calculate(x)).Sum();
    }
}
