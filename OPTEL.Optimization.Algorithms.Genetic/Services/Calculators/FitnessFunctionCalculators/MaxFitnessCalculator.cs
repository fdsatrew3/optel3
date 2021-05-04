using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.FitnessFunctionCalculators
{
    public class MaxFitnessCalculator : IFitnessCalculator<ProductionPlan>
    {
        public double Calculate(ProductionPlan individual)
        {
            return individual.TargetFunctionValue;
        }
    }
}
