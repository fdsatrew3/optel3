using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CoolingLipChangesExcelParser : DatabaseExcelParcerBase<CoolingLipChange>
    {
        protected override int WorkSheetIndex => 9;

        private enum ColumnIndexes { ParentProductionLine, CoolingLipToChange, ReconfigurationTime, Consumption }

        public CoolingLipChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override CoolingLipChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new CoolingLipChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentProductionLine)),
                CoolingLipToChange = excelDataReader.GetDouble(ColumnIndexes.CoolingLipToChange),
                ReconfigurationTime = excelDataReader.GetDouble(ColumnIndexes.ReconfigurationTime),
                Consumption = excelDataReader.GetDouble(ColumnIndexes.Consumption)
            };
        }
    }
}
