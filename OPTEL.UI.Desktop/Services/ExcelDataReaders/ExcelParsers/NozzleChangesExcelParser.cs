using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class NozzleChangesExcelParser : DatabaseExcelParcerBase<NozzleChange>
    {
        protected override int WorkSheetIndex => 7;

        private enum ColumnIndexes { ParentProductionLine, NozzleToChange, ReconfigurationTime, Consumption }

        public NozzleChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override NozzleChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new NozzleChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentProductionLine)),
                NozzleToChange = excelDataReader.GetDouble(ColumnIndexes.NozzleToChange),
                ReconfigurationTime = excelDataReader.GetDouble(ColumnIndexes.ReconfigurationTime),
                Consumption = excelDataReader.GetDouble(ColumnIndexes.Consumption)
            };
        }
    }
}
