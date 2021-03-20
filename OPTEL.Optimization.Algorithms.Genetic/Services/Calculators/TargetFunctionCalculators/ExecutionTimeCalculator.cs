using System;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators
{
    public class ExecutionTimeCalculator : IExecutionTimeCalculator
    {
        public IOrderExcecutionTimeCalculator OrderExcecutionTimeCalculator { get; }

        public ExecutionTimeCalculator(IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator)
        {
            OrderExcecutionTimeCalculator = orderExcecutionTimeCalculator ?? throw new ArgumentNullException(nameof(orderExcecutionTimeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
        {
            var result = 0d;

            foreach (var order in productionLineQueue.Orders)
            {
                result += OrderExcecutionTimeCalculator.Calculate(order);
            }

            return result;
        }
    }
}
