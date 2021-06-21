using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base
{
    public interface IOrderPositionSwaper
    {
        void Swap(ProductionPlan productionPlan, OrderPosition firstElement, OrderPosition secondElement);
    }
}
