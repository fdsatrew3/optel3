using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base
{
    public interface IOrderCostCalculator
    {
        double Calculate(Order order);
    }
}
