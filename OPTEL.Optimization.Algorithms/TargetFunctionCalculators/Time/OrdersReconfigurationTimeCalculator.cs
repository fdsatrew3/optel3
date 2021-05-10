using System.Diagnostics;
using System.Linq;
using OPTEL.Data;

using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time
{
    public class OrdersReconfigurationTimeCalculator : IOrdersReconfigurationTimeCalculator
    {
        public double Calculate(ProductionLine productionLine, Order orderFrom, Order orderTo)
        {
            double result = 0;

            if (orderFrom.FilmRecipe.FilmType != orderTo.FilmRecipe.FilmType)
            {
                var change = productionLine.FilmTypesChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.FilmTypeFrom == orderFrom.FilmRecipe.FilmType && x.FilmTypeTo == orderTo.FilmRecipe.FilmType);

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from film type {0} to {1}", orderFrom.FilmRecipe.FilmType.Article, orderTo.FilmRecipe.FilmType.Article));
                }
            }

            if (!orderFrom.FilmRecipe.CoolingLip.Equals(orderTo.FilmRecipe.CoolingLip))
            {
                var change = productionLine.CoolingLipChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.CoolingLipToChange.Equals(orderTo.FilmRecipe.CoolingLip));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip));
                }
            }

            if (!orderFrom.FilmRecipe.Calibration.Equals(orderTo.FilmRecipe.Calibration))
            {
                var change = productionLine.CalibrationChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.CalibrationToChange.Equals(orderTo.FilmRecipe.Calibration));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration));
                }
            }

            if (!orderFrom.FilmRecipe.Nozzle.Equals(orderTo.FilmRecipe.Nozzle))
            {
                var change = productionLine.NozzleChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => x.NozzleToChange.Equals(orderTo.FilmRecipe.Nozzle));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle));
                }
            }

            if (!orderFrom.Width.Equals(orderTo.Width))
                result += productionLine.WidthChangeTime;

            if (!orderFrom.FilmRecipe.Thickness.Equals(orderTo.FilmRecipe.Thickness))
                result += productionLine.ThicknessChangeTime;

            return result;
        }
    }
}
