using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base
{
    public interface IExecutionTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
