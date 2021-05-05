using System.Linq;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Util
{
    public class OrderPositionSwaper : IOrderPositionSwaper
    {
        public void Swap(ProductionPlan productionPlan, OrderPosition firstElement, OrderPosition secondElement)
        {
            var queueFirst = productionPlan.ProductionLineQueues.First(x => x.ProductionLine == firstElement.ProductionLine);
            var queueSecond = productionPlan.ProductionLineQueues.First(x => x.ProductionLine == secondElement.ProductionLine);

            queueFirst.Orders.Remove(firstElement.Order);
            queueSecond.Orders.Remove(secondElement.Order);

            queueFirst.Orders.Insert(firstElement.Position, secondElement.Order);
            queueFirst.Orders.Insert(secondElement.Position, firstElement.Order);
        }
    }
}
