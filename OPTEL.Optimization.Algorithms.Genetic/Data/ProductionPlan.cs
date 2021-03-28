using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionPlan : ICalculatedIndividual, ICloneableIndividual
    {
        public List<ProductionLineQueue> ProductionLineQueues { get; set; }

        public double TargetFunctionValue => _targetFunctionValue ?? (_targetFunctionValue = _targetFunctionCalculator.Calculate(this)).Value;

        public double FitnessFunctionValue => _fitnessFunctionValue ?? (_fitnessFunctionValue = _fitnessFunctionCalculator.Calculate(this)).Value;

        private double? _targetFunctionValue;
        private double? _fitnessFunctionValue;
        private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;
        private readonly IFitnessFunctionCalculator<ProductionPlan> _fitnessFunctionCalculator;

        public ProductionPlan(ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator, IFitnessFunctionCalculator<ProductionPlan> fitnessFunctionCalculator)
        {
            _targetFunctionCalculator = targetFunctionCalculator ?? throw new System.ArgumentNullException(nameof(targetFunctionCalculator));
            _fitnessFunctionCalculator = fitnessFunctionCalculator ?? throw new System.ArgumentNullException(nameof(fitnessFunctionCalculator));
        }

        public object Clone()
        {
            return new ProductionPlan(_targetFunctionCalculator, _fitnessFunctionCalculator) { ProductionLineQueues = ProductionLineQueues.Select(x => x.Clone() as ProductionLineQueue).ToList() };
        }
    }
}
