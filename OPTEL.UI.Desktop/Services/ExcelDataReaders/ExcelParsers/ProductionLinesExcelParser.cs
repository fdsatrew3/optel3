using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class ProductionLinesExcelParser : ExcelParcerBase<ProductionLine>
    {
        protected override int WorkSheetIndex => 5;

        private enum ColumnIndexes { Name, Code, HourCost, MaxProductionSpeed, WidthMin, WidthMax, ThicknessMin, ThicknessMax, WeightMin, WeightMax, LengthMin, LengthMax, ThicknessChangeTime, WidthChangeTime }

        protected override ProductionLine ParseRow(IExcelDataReader excelDataReader)
        {
            return new ProductionLine
            {
                Name = excelDataReader.GetFormattedValue(ColumnIndexes.Name),
                Code = excelDataReader.GetFormattedValue(ColumnIndexes.Code),
                HourCost = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.HourCost)),
                MaxProductionSpeed = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.MaxProductionSpeed)),
                WidthMin = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.WidthMin)),
                WidthMax = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.WidthMax)),
                ThicknessMin = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ThicknessMin)),
                ThicknessMax = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ThicknessMax)),
                WeightMin = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.WeightMin)),
                WeightMax = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.WeightMax)),
                LengthMin = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.LengthMin)),
                LengthMax = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.LengthMax)),
                ThicknessChangeTime = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ThicknessChangeTime)),
                WidthChangeTime = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.WidthChangeTime))
            };
        }
    }
}
