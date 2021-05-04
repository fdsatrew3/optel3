using System;
using System.Linq;
using Optimization.Algorithms;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Cost.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Cost
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
