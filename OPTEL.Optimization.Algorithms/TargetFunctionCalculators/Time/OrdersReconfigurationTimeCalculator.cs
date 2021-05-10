using System.Diagnostics;
using System;
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

            if (!IsEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
            {
                var change = productionLine.CoolingLipChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => IsEqual(x.CoolingLipToChange, orderTo.FilmRecipe.CoolingLip));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip));
                }
            }

            if (!IsEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
            {
                var change = productionLine.CalibrationChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => IsEqual(x.CalibrationToChange, orderTo.FilmRecipe.Calibration));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration));
                }
            }

            if (!IsEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
            {
                var change = productionLine.NozzleChanges?
                    .Where(x => x.ParentProductionLine == productionLine)?
                    .FirstOrDefault(x => IsEqual(x.NozzleToChange, orderTo.FilmRecipe.Nozzle));

                if (change != null)
                {
                    result += change.ReconfigurationTime;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find reconfiguration time from cooling lip {0} to {1}", orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle));
                }
            }

            if (!IsEqual(orderFrom.Width, orderTo.Width))
                result += productionLine.WidthChangeTime;

            if (!IsEqual(orderFrom.FilmRecipe.Thickness, orderTo.FilmRecipe.Thickness))
                result += productionLine.ThicknessChangeTime;

            return result;
        }

        private static bool IsEqual(double value1, double value2)
        {
            double difference = Math.Abs(value1 * .00001);

            return Math.Abs(value1 - value2) <= difference;
        }
    }
}
