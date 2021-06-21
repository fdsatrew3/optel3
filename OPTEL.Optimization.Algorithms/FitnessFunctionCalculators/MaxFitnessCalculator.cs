using OPTEL.Data;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FitnessFunctionCalculators
{
    public class MaxFitnessCalculator<T> : IFitnessCalculator<T>
        where T : ProductionPlan
    {
        private readonly ITargetFunctionCalculator<T> _targetFunctionCalculator;

        public MaxFitnessCalculator(ITargetFunctionCalculator<T> targetFunctionCalculator)
        {
            _targetFunctionCalculator = targetFunctionCalculator ?? throw new System.ArgumentNullException(nameof(targetFunctionCalculator));
        }

        public double Calculate(T individual)
        {
            return _targetFunctionCalculator.Calculate(individual);
        }
    }
}
