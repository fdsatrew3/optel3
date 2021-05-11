using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using System;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
{
    public class OrderExcecutionTimeCalculator : IOrderExcecutionTimeCalculator
    {
        public double Calculate(Order order)
        {
            if (order.PredefinedTime != 0)
            {
                return Convert.ToDouble(order.PredefinedTime) * 60;
            }
            else
            {
                return Convert.ToDouble(order.QuantityInRunningMeter / (order.FilmRecipe.ProductionSpeed / 60));
            }
        }
    }
}
