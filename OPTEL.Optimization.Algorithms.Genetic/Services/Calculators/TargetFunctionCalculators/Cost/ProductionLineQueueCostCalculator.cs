﻿using System;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Cost.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Cost
{
    public class ProductionLineQueueCostCalculator : IProductionLineQueueCostCalculator
    {
        public IProductionLineQueueTimeCalculator TimeCalculator { get; }

        public ProductionLineQueueCostCalculator(IProductionLineQueueTimeCalculator timeCalculator)
        {
            TimeCalculator = timeCalculator ?? throw new ArgumentNullException(nameof(timeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue) => TimeCalculator.Calculate(productionLineQueue) * Convert.ToDouble(productionLineQueue.Extruder.HourCost);
    }
}
