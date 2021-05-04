using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base
{
    public interface IProductionLineQueueCostCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
