using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

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
                NozzleToChange = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.NozzleToChange)),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ReconfigurationTime)),
                Consumption = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.Consumption))
            };
        }
    }
}
