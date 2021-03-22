using System;
using Optimization.Algorithms.Genetic.Data;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time
{
    public class TimeFunctionCalculator : ITargetFunctionCalculator<ProductionPlan>
    {
        public IProductionLineQueueTimeCalculator ProductionLineQueueTimeCalculator { get; }

        public TimeFunctionCalculator(IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
        {
            ProductionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueTimeCalculator));
        }

        public double Calculate(ProductionPlan individual)
        {
            var result = 0d;

            foreach (var queue in individual.ProductionLineQueues)
            {
                result += ProductionLineQueueTimeCalculator.Calculate(queue);
            }

            return result;
        }
    }
}
