using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class OrderCostCalculator : IOrderCostCalculator
    {
        public double Calculate(Order order) => order.Weight * order.FilmRecipe.MaterialCost;
    }
}
