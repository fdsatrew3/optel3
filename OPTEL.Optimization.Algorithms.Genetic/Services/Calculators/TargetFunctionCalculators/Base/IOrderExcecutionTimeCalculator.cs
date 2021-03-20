using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Base
{
    public interface IOrderExcecutionTimeCalculator
    {
        double Calculate(Order order);
    }
}
