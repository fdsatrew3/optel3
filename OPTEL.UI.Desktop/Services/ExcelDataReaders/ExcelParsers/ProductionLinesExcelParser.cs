using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
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
                Name = excelDataReader.GetValue((int)ColumnIndexes.Name).ToString(),
                Code = excelDataReader.GetValue((int)ColumnIndexes.Code).ToString(),
                HourCost = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.HourCost)),
                MaxProductionSpeed = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.MaxProductionSpeed)),
                WidthMin = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.WidthMin)),
                WidthMax = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.WidthMax)),
                ThicknessMin = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ThicknessMin)),
                ThicknessMax = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ThicknessMax)),
                WeightMin = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.WeightMin)),
                WeightMax = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.WeightMax)),
                LengthMin = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.LengthMin)),
                LengthMax = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.LengthMax)),
                ThicknessChangeTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ThicknessChangeTime)),
                WidthChangeTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.WidthChangeTime))
            };
        }
    }
}
