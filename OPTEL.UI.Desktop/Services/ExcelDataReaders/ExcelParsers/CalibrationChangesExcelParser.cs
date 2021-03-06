using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CalibrationChangesExcelParser : DatabaseExcelParcerBase<CalibrationChange>
    {
        protected override int WorkSheetIndex => 8;

        private enum ColumnIndexes { ParentProductionLine, CalibrationToChange, ReconfigurationTime, Consumption }

        public CalibrationChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override CalibrationChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new CalibrationChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentProductionLine)),
                CalibrationToChange = excelDataReader.GetDouble(ColumnIndexes.CalibrationToChange),
                ReconfigurationTime = excelDataReader.GetDouble(ColumnIndexes.ReconfigurationTime),
                Consumption = excelDataReader.GetDouble(ColumnIndexes.Consumption)
            };
        }
    }
}
