using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class FilmTypesExcelParser : ExcelParcerBase<FilmType>
    {
        protected override int WorkSheetIndex => 1;

        private enum ColumnIndexes { Article }

        protected override FilmType ParseRow(IExcelDataReader excelDataReader)
        {
            return new FilmType()
            {
                Article = excelDataReader.GetFormattedValue(ColumnIndexes.Article)
            };
        }
    }
}
