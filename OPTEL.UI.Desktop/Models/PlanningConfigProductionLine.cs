using OPTEL.Data;
using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class PlanningConfigProductionLine : ProductionLine, IDataModel
    {
        public bool IsSelected { get; set; }

        public PlanningConfigProductionLine(ProductionLine line)
        {
            CalibrationChanges = line.CalibrationChanges;
            Code = line.Code;
            CoolingLipChanges = line.CoolingLipChanges;
            FilmTypesChanges = line.FilmTypesChanges;
            HourCost = line.HourCost;
            ID = line.ID;
            LengthMax = line.LengthMax;
            LengthMin = line.LengthMin;
            MaxProductionSpeed = line.MaxProductionSpeed;
            Name = line.Name;
            NozzleChanges = line.NozzleChanges;
            ThicknessChangeTime = line.ThicknessChangeTime;
            ThicknessMax = line.ThicknessMax;
            ThicknessMin = line.ThicknessMin;
            WeightMin = line.WeightMin;
            WeightMax = line.WeightMax;
            WidthChangeTime = line.WidthChangeTime;
            WidthMax = line.WidthMax;
            WidthMin = line.WidthMin;
            IsSelected = false;
        }
    }
}
