using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base
{
    public interface IProductionLineQueueTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
