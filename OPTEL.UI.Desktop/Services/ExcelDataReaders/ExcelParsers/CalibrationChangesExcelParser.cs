using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CalibrationChangesExcelParser : DatabaseExcelParcerBase<CalibrationChange>
    {
        protected override int WorkSheetIndex => 8;

        private enum ColumnIndexes { ParentProductionLine, CalibrationToChange, ReconfigurationTime }

        public CalibrationChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override CalibrationChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new CalibrationChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.ParentProductionLine).ToString()),
                CalibrationToChange = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.CalibrationToChange)),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ReconfigurationTime))
            };
        }
    }
}
