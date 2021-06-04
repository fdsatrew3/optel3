using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class ProductionLinesExcelParser : ExcelParcerBase<ProductionLine>
    {
        protected override int WorkSheetIndex => 5;

        private enum ColumnIndexes { Name, Code, HourCost, MaxProductionSpeed, WidthMin, WidthMax, ThicknessMin, ThicknessMax, WeightMin, WeightMax, LengthMin, LengthMax, ThicknessChangeTime, ThicknessChangeConsumption, WidthChangeTime, WidthChangeConsumption }

        protected override ProductionLine ParseRow(IExcelDataReader excelDataReader)
        {
            return new ProductionLine
            {
                Name = excelDataReader.GetFormattedValue(ColumnIndexes.Name),
                Code = excelDataReader.GetFormattedValue(ColumnIndexes.Code),
                HourCost = excelDataReader.GetDouble(ColumnIndexes.HourCost),
                MaxProductionSpeed = excelDataReader.GetDouble(ColumnIndexes.MaxProductionSpeed),
                WidthMin = excelDataReader.GetDouble(ColumnIndexes.WidthMin),
                WidthMax = excelDataReader.GetDouble(ColumnIndexes.WidthMax),
                ThicknessMin = excelDataReader.GetDouble(ColumnIndexes.ThicknessMin),
                ThicknessMax = excelDataReader.GetDouble(ColumnIndexes.ThicknessMax),
                WeightMin = excelDataReader.GetDouble(ColumnIndexes.WeightMin),
                WeightMax = excelDataReader.GetDouble(ColumnIndexes.WeightMax),
                LengthMin = excelDataReader.GetDouble(ColumnIndexes.LengthMin),
                LengthMax = excelDataReader.GetDouble(ColumnIndexes.LengthMax),
                ThicknessChangeTime = excelDataReader.GetDouble(ColumnIndexes.ThicknessChangeTime),
                ThicknessChangeConsumption = excelDataReader.GetDouble(ColumnIndexes.ThicknessChangeConsumption),
                WidthChangeTime = excelDataReader.GetDouble(ColumnIndexes.WidthChangeTime),
                WidthChangeConsumption = excelDataReader.GetDouble(ColumnIndexes.WidthChangeConsumption)
            };
        }
    }
}
