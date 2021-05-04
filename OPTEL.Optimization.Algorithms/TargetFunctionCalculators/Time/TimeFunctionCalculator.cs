using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using Optimization.Algorithms;
using System;
using System.Linq;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
{
    public class TimeFunctionCalculator : ITargetFunctionCalculator<ProductionPlan>
    {
        public IProductionLineQueueTimeCalculator ProductionLineQueueTimeCalculator { get; }

        public TimeFunctionCalculator(IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
        {
            ProductionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueTimeCalculator));
        }

        public double Calculate(ProductionPlan individual) => individual.ProductionLineQueues.Select(x => ProductionLineQueueTimeCalculator.Calculate(x)).Max();
    }
}
