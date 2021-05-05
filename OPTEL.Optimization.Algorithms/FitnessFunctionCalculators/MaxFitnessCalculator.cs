using OPTEL.Data;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FitnessFunctionCalculators
{
    public class MaxFitnessCalculator : IFitnessCalculator<ProductionPlan>
    {
        private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;

        public MaxFitnessCalculator(ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator)
        {
            _targetFunctionCalculator = targetFunctionCalculator ?? throw new System.ArgumentNullException(nameof(targetFunctionCalculator));
        }

        public double Calculate(ProductionPlan individual)
        {
            return _targetFunctionCalculator.Calculate(individual);
        }
    }
}
