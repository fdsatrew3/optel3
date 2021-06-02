using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base
{
    public interface IOrdersReconfigurationCostCalculator
    {
        double Calculate(ProductionLine productionLine, Order orderFrom, Order orderTo);
    }
}
