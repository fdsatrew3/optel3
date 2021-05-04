﻿using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Cost.Base
{
    public interface IProductionLineQueueCostCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
