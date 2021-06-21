using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base
{
    public interface IReconfigurationCostCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
