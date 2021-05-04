using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base
{
    public interface IReconfigurationTimeCalculator
    {
        double Calculate(ProductionLineQueue productionLineQueue);
    }
}
