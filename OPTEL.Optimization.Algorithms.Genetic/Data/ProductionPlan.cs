using System.Linq;
using Optimization.Algorithms;
using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionPlan : OPTEL.Data.ProductionPlan, ICalculatedIndividual, ICloneableIndividual
    {
        public double TargetFunctionValue => _targetFunctionValue ?? (_targetFunctionValue = _targetFunctionCalculator.Calculate(this)).Value;

        public double FitnessFunctionValue => _fitnessFunctionValue ?? (_fitnessFunctionValue = _fitnessCalculator.Calculate(this)).Value;

        private double? _targetFunctionValue;
        private double? _fitnessFunctionValue;
        private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;
        private readonly IFitnessCalculator<ProductionPlan> _fitnessCalculator;

        public ProductionPlan(ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator, IFitnessCalculator<ProductionPlan> fitnessCalculator)
        {
            _targetFunctionCalculator = targetFunctionCalculator ?? throw new System.ArgumentNullException(nameof(targetFunctionCalculator));
            _fitnessCalculator = fitnessCalculator ?? throw new System.ArgumentNullException(nameof(fitnessCalculator));
        }

        public object Clone()
        {
            return new ProductionPlan(_targetFunctionCalculator, _fitnessCalculator) { ProductionLineQueues = ProductionLineQueues.Select(x => new OPTEL.Data.ProductionLineQueue { ProductionLine = x.ProductionLine, Orders = x.Orders.ToList() }).ToList() };
        }
    }
}
