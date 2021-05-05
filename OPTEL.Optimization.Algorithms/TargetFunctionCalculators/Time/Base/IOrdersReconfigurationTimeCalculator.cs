using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base
{
    public interface IOrdersReconfigurationTimeCalculator
    {
        double Calculate(ProductionLine productionLine, Order orderFrom, Order orderTo);
    }
}
