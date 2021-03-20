using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Base
{
    public interface IOrdersReconfigurationTimeCalculator
    {
        double Calculate(Extruder productionLine, Order orderFrom, Order orderTo);
    }
}
