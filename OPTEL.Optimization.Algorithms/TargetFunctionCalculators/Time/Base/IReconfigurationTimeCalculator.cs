using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base
{
    public interface IReconfigurationTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
