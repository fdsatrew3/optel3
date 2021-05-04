using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base
{
    public interface IOrderExcecutionTimeCalculator
    {
        double Calculate(Order order);
    }
}
