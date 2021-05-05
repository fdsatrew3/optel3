﻿using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.FitnessFunctionCalculators
{
    public class MinFitnessCalculator : IFitnessCalculator<ProductionPlan>
    {
        public double Calculate(ProductionPlan individual)
        {
            return -individual.TargetFunctionValue;
        }
    }
}
