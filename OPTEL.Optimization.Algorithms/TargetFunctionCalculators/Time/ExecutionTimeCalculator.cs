using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using System;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
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
