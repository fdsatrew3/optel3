using Optimization.Algorithms.Genetic.Core;

using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.FitnessFunctionCalculators
{
    public class MinFitnessFunctionCalculator : IFitnessFunctionCalculator<ProductionPlan>
    {
        public double Calculate(ProductionPlan individual)
        {
            return -individual.TargetFunctionValue;
        }
    }
}
