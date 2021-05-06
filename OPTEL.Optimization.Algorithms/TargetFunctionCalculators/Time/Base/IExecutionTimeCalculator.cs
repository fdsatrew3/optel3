using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base
{
    public interface IExecutionTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
