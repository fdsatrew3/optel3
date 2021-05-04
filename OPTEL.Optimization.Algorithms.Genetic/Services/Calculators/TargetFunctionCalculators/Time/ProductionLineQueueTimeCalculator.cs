﻿using System;
using OPTEL.Data;

using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time
{
    public class ProductionLineQueueTimeCalculator : IProductionLineQueueTimeCalculator
    {
        public IExecutionTimeCalculator ExecutionTimeCalculator { get; }

        public IReconfigurationTimeCalculator ReconfigurationTimeCalculator { get; }

        public ProductionLineQueueTimeCalculator(IExecutionTimeCalculator executionTimeCalculator, IReconfigurationTimeCalculator reconfigurationTimeCalculator)
        {
            ExecutionTimeCalculator = executionTimeCalculator ?? throw new ArgumentNullException(nameof(executionTimeCalculator));
            ReconfigurationTimeCalculator = reconfigurationTimeCalculator ?? throw new ArgumentNullException(nameof(reconfigurationTimeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
        {
            var executionTime = ExecutionTimeCalculator.Calculate(productionLineQueue);
            var reconfigurationTime = ReconfigurationTimeCalculator.Calculate(productionLineQueue);

            return executionTime + reconfigurationTime;
        }
    }
}
