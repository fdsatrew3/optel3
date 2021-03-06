using System;
using System.Diagnostics;
using System.Linq;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost.Base;

namespace OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost
{
    public class OrdersReconfigurationCostCalculator : IOrdersReconfigurationCostCalculator
    {
        public double Calculate(ProductionLine productionLine, Order orderFrom, Order orderTo)
        {
            double result = 0;

            if (orderFrom.FilmRecipe.FilmType != orderTo.FilmRecipe.FilmType)
            {
                var change = productionLine.FilmTypesChanges?
                    .FirstOrDefault(x => x.FilmTypeFrom == orderFrom.FilmRecipe.FilmType && x.FilmTypeTo == orderTo.FilmRecipe.FilmType);

                if (change != null)
                {
                    result += change.Consumption;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find consumption from film type {0} to {1}", orderFrom.FilmRecipe.FilmType.Article, orderTo.FilmRecipe.FilmType.Article));
                }
            }

            if (!IsEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
            {
                var change = productionLine.CoolingLipChanges?
                    .FirstOrDefault(x => IsEqual(x.CoolingLipToChange, orderTo.FilmRecipe.CoolingLip));

                if (change != null)
                {
                    result += change.Consumption;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find consumption from cooling lip {0} to {1}", orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip));
                }
            }

            if (!IsEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
            {
                var change = productionLine.CalibrationChanges?
                    .FirstOrDefault(x => IsEqual(x.CalibrationToChange, orderTo.FilmRecipe.Calibration));

                if (change != null)
                {
                    result += change.Consumption;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find consumption from calibration {0} to {1}", orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration));
                }
            }

            if (!IsEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
            {
                var change = productionLine.NozzleChanges?
                    .FirstOrDefault(x => IsEqual(x.NozzleToChange, orderTo.FilmRecipe.Nozzle));

                if (change != null)
                {
                    result += change.Consumption;
                }
                else
                {
                    Debug.WriteLine(string.Format("Can't find consumption from nozzle {0} to {1}", orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle));
                }
            }

            if (!IsEqual(orderFrom.Width, orderTo.Width))
                result += productionLine.WidthChangeConsumption;

            if (!IsEqual(orderFrom.FilmRecipe.Thickness, orderTo.FilmRecipe.Thickness))
                result += productionLine.ThicknessChangeConsumption;

            return result;
        }

        private static bool IsEqual(double value1, double value2)
        {
            double difference = Math.Abs(value1 * .00001);

            return Math.Abs(value1 - value2) <= difference;
        }
    }
}
