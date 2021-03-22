using System;
using OPTEL.Data;

using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time
{
    public class OrderExcecutionTimeCalculator : IOrderExcecutionTimeCalculator
    {
        public double Calculate(Order order)
        {
            if (order.PredefinedTime != 0)
            {
                return Convert.ToDouble(order.PredefinedTime);
            }
            else
            {
                return Convert.ToDouble(order.QuantityInRunningMeter / (order.FilmRecipe.ProductionSpeed / 60));
            }
        }
    }
}
