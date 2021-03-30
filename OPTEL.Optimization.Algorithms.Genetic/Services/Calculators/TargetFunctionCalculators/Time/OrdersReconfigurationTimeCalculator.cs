using System;
using System.Linq;
using OPTEL.Data;

using OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Calculators.TargetFunctionCalculators.Time
{
    public class OrdersReconfigurationTimeCalculator : IOrdersReconfigurationTimeCalculator
    {
        public double Calculate(ProductionLine productionLine, Order orderFrom, Order orderTo)
        {
            decimal result = 0;

            if (orderFrom.FilmRecipe.FilmType != orderTo.FilmRecipe.FilmType)
            {
                var change = productionLine.FilmTypesChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.FilmTypeFrom == orderFrom.FilmRecipe.FilmType && x.FilmTypeTo == orderTo.FilmRecipe.FilmType);

                if (productionLine != null)
                    result += change.ReconfigurationTime;
            }

            if (!orderFrom.FilmRecipe.CoolingLip.Equals(orderTo.FilmRecipe.CoolingLip))
            {
                var change = productionLine.CoolingLipChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.CoolingLipToChange.Equals(orderTo.FilmRecipe.CoolingLip));

                if (productionLine != null)
                    result += change.ReconfigurationTime;
            }

            if (!orderFrom.FilmRecipe.Calibration.Equals(orderTo.FilmRecipe.Calibration))
            {
                var change = productionLine.CalibrationChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.CalibrationToChange.Equals(orderTo.FilmRecipe.Calibration));

                if (productionLine != null)
                    result += change.ReconfigurationTime;
            }

            if (!orderFrom.FilmRecipe.Nozzle.Equals(orderTo.FilmRecipe.Nozzle))
            {
                var change = productionLine.NozzleChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.NozzleToChange.Equals(orderTo.FilmRecipe.Nozzle));

                if (productionLine != null)
                    result += change.ReconfigurationTime;
            }

            if (!orderFrom.Width.Equals(orderTo.Width))
                result += productionLine.WidthChangeTime;

            if (!orderFrom.FilmRecipe.Thickness.Equals(orderTo.FilmRecipe.Thickness))
                result += productionLine.ThicknessChangeTime;

            return Convert.ToDouble(result);
        }
    }
}
