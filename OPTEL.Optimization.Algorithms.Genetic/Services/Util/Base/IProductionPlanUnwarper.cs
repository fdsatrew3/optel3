using System.Collections.Generic;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base
{
    public interface IProductionPlanUnwarper
    {
        ICollection<OrderPosition> Unwarp(ProductionPlan productionPlan);
    }
}
