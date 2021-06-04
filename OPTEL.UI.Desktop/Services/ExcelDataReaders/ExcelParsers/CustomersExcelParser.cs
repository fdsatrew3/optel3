using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CustomersExcelParser : ExcelParcerBase<Customer>
    {
        protected override int WorkSheetIndex => 3;

        private enum ColumnIndexes { Name, Number }

        protected override Customer ParseRow(IExcelDataReader excelDataReader)
        {
            return new Customer
            {
                Name = excelDataReader.GetString(ColumnIndexes.Name),
                Number = excelDataReader.GetString(ColumnIndexes.Number)
            };
        }
    }
}
