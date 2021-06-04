using System;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
{
    public class OrderExcecutionTimeCalculator : IOrderExcecutionTimeCalculator
    {
        /// <summary>
        /// Calculate order execution time, min
        /// </summary>
        /// <param name="order">Order to calculate</param>
        /// <returns>Order calculation time, min</returns>
        public double Calculate(Order order)
        {
            if (order.PredefinedTime != null)
            {
                return Convert.ToDouble(order.PredefinedTime);
            }
            else
            {
                return Convert.ToDouble(order.QuantityInRunningMeter / order.FilmRecipe.ProductionSpeed);
            }
        }
    }
}
