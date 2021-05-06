using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CoolingLipChangesExcelParser : DatabaseExcelParcerBase<CoolingLipChange>
    {
        protected override int WorkSheetIndex => 8;

        private enum ColumnIndexes { ParentProductionLine, CoolingLipToChange, ReconfigurationTime }

        public CoolingLipChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override CoolingLipChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new CoolingLipChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.ParentProductionLine).ToString()),
                CoolingLipToChange = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.CoolingLipToChange)),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ReconfigurationTime))
            };
        }
    }
}
