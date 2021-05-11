using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;

namespace OPTEL.UI.Desktop.Services.ModelsConverter
{
    public class PlanningConfigProductionLineToProductionLineConverterService : IModelConverterService<PlanningConfigProductionLine, ProductionLine>
    {
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public PlanningConfigProductionLine Convert(ProductionLine source)
        {
            return new PlanningConfigProductionLine(source);
        }

        public ProductionLine ConvertBack(PlanningConfigProductionLine source)
        {
            ProductionLine line = new ProductionLine
            {
                CalibrationChanges = source.CalibrationChanges,
                Code = source.Code,
                CoolingLipChanges = source.CoolingLipChanges,
                FilmTypesChanges = source.FilmTypesChanges,
                HourCost = source.HourCost,
                ID = source.ID,
                LengthMax = source.LengthMax,
                LengthMin = source.LengthMin,
                MaxProductionSpeed = source.MaxProductionSpeed,
                Name = source.Name,
                NozzleChanges = source.NozzleChanges,
                ThicknessChangeTime = source.ThicknessChangeTime,
                ThicknessMax = source.ThicknessMax,
                ThicknessMin = source.ThicknessMin,
                WeightMin = source.WeightMin,
                WeightMax = source.WeightMax,
                WidthChangeTime = source.WidthChangeTime,
                WidthMax = source.WidthMax,
                WidthMin = source.WidthMin
            };
            return line;
        }
    }
}
