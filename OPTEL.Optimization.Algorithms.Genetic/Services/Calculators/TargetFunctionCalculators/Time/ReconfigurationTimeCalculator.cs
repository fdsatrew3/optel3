﻿using System;
using System.Linq;
using OPTEL.Data;

using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time
{
    public class ReconfigurationTimeCalculator : IReconfigurationTimeCalculator
    {
        public IOrdersReconfigurationTimeCalculator OrdersReconfigurationTimeCalculator { get; }

        public ReconfigurationTimeCalculator(IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator)
        {
            OrdersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator ?? throw new ArgumentNullException(nameof(ordersReconfigurationTimeCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
        {
            var result = 0d;

            for (int i = 0; i < productionLineQueue.Orders.Count - 1; i++)
            {
                result += OrdersReconfigurationTimeCalculator.Calculate(productionLineQueue.ProductionLine,
                    productionLineQueue.Orders.ElementAt(i),
                    productionLineQueue.Orders.ElementAt(i + 1));
            }

            return result;
        }
    }
}
