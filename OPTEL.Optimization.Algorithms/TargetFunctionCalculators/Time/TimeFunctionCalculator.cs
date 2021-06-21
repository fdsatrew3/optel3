using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using Optimization.Algorithms;
using System;
using System.Linq;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
{
    public class TimeFunctionCalculator<T> : ITargetFunctionCalculator<T>
        where T : ProductionPlan
    {
        public IProductionLineQueueTimeCalculator ProductionLineQueueTimeCalculator { get; }

        public TimeFunctionCalculator(IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
        {
            ProductionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueTimeCalculator));
        }

        public double Calculate(T individual) => individual.ProductionLineQueues.Select(x => ProductionLineQueueTimeCalculator.Calculate(x)).Max();
    }
}
