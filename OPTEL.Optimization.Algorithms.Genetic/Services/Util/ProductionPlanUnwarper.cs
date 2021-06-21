using System.Collections.Generic;
using System.Linq;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Util
{
    public class ProductionPlanUnwarper : IProductionPlanUnwarper
    {
        public ICollection<OrderPosition> Unwarp(ProductionPlan productionPlan)
        {
            var result = new List<OrderPosition>();

            foreach (var productionLineQueue in productionPlan.ProductionLineQueues)
            {
                for (int i = 0; i < productionLineQueue.Orders.Count; i++)
                {
                    result.Add(new OrderPosition { ProductionLine = productionLineQueue.ProductionLine, Order = productionLineQueue.Orders.ElementAt(i), Position = i });
                }
            }

            return result;
        }
    }
}
