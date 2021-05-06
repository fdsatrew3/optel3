using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class FilmTypesChangesExcelParser : DatabaseExcelParcerBase<FilmTypesChange>
    {
        protected override int WorkSheetIndex => 6;

        private enum ColumnIndexes { ParentProductionLine, FilmTypeFrom, FilmTypeTo, ReconfigurationTime }

        public FilmTypesChangesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override FilmTypesChange ParseRow(IExcelDataReader excelDataReader)
        {
            return new FilmTypesChange
            {
                ParentProductionLine = UnitOfWork.ProductionLineRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.ParentProductionLine).ToString()),
                FilmTypeFrom = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetValue((int)ColumnIndexes.FilmTypeFrom).ToString()),
                FilmTypeTo = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetValue((int)ColumnIndexes.FilmTypeTo).ToString()),
                ReconfigurationTime = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ReconfigurationTime))
            };
        }
    }
}
