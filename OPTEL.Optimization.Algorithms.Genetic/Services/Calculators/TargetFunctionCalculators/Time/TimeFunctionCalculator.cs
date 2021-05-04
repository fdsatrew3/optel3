using System;
using System.Linq;
using Optimization.Algorithms;

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

        public double Calculate(ProductionPlan individual) => individual.ProductionLineQueues.Select(x => ProductionLineQueueTimeCalculator.Calculate(x)).Max();
    }
}
