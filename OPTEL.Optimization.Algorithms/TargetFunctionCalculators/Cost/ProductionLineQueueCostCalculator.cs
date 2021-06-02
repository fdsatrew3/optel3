using System;
using System.Linq;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class ProductionLineQueueCostCalculator : IProductionLineQueueCostCalculator
    {
        private readonly IProductionLineQueueTimeCalculator _timeCalculator;
        private readonly IOrderCostCalculator _orderCostCalculator;
        private readonly IReconfigurationCostCalculator _reconfigurationCostCalculator;

        public ProductionLineQueueCostCalculator(IProductionLineQueueTimeCalculator timeCalculator, 
            IOrderCostCalculator orderCostCalculator, 
            IReconfigurationCostCalculator reconfigurationCostCalculator)
        {
            _timeCalculator = timeCalculator ?? throw new ArgumentNullException(nameof(timeCalculator));
            _orderCostCalculator = orderCostCalculator ?? throw new ArgumentNullException(nameof(orderCostCalculator));
            _reconfigurationCostCalculator = reconfigurationCostCalculator ?? throw new ArgumentNullException(nameof(reconfigurationCostCalculator));
        }

        public double Calculate(ProductionLineQueue productionLineQueue)
            => (_timeCalculator.Calculate(productionLineQueue) / 60) 
            * Convert.ToDouble(productionLineQueue.ProductionLine.HourCost) 
            + productionLineQueue.Orders.Sum(x => _orderCostCalculator.Calculate(x))
            + _reconfigurationCostCalculator.Calculate(productionLineQueue);
    }
}
