using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class FilmTypesChangesExcelParser : DatabaseExcelParcerBase<FilmTypesChange>
    {
        protected override int WorkSheetIndex => 6;

        private enum ColumnIndexes { ParentProductionLine, FilmTypeFrom, FilmTypeTo, ReconfigurationTime, Consumption }

        public FilmTypesChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override FilmTypesChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new FilmTypesChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentProductionLine)),
                FilmTypeFrom = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetFormattedValue(ColumnIndexes.FilmTypeFrom)),
                FilmTypeTo = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetFormattedValue(ColumnIndexes.FilmTypeTo)),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ReconfigurationTime)),
                Consumption = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.Consumption))
            };
        }
    }
}
