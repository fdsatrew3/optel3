using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class NozzleChangesExcelParser : DatabaseExcelParcerBase<NozzleChange>
    {
        protected override int WorkSheetIndex => 7;

        private enum ColumnIndexes { ParentProductionLine, NozzleToChange, ReconfigurationTime }

        public NozzleChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override NozzleChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new NozzleChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.ParentProductionLine).ToString()),
                NozzleToChange = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.NozzleToChange)),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ReconfigurationTime))
            };
        }
    }
}
