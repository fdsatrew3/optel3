using Optimization.Algorithms.Genetic.Data;
using System.Collections.Generic;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionPlan : ICalculatedIndividual
    {
        public List<ProductionLineQueue> ProductionLineQueues { get; set; }

        public double TargetFunctionValue => throw new System.NotImplementedException();

        public double FitnessFunctionValue => throw new System.NotImplementedException();
    }
}
