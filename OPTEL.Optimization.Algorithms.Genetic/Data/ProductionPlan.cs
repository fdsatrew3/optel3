using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionPlan : ICalculatedIndividual, ICloneableIndividual
    {
        public List<ProductionLineQueue> ProductionLineQueues { get; set; }

        public double TargetFunctionValue => throw new System.NotImplementedException();

        public double FitnessFunctionValue => throw new System.NotImplementedException();

        public object Clone()
        {
            return new ProductionPlan { ProductionLineQueues = ProductionLineQueues.Select(x => x.Clone() as ProductionLineQueue).ToList() };
        }
    }
}
