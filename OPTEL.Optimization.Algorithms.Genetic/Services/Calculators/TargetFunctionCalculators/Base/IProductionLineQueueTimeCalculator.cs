﻿using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Base
{
    public interface IProductionLineQueueTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
