using ExcelDataReader;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class CustomersExcelParser : ExcelParcerBase<Customer>
    {
        protected override int WorkSheetIndex => 3;

        private enum ColumnIndexes { Name }

        protected override Customer ParseRow(IExcelDataReader excelDataReader)
        {
            return new Customer
            {
                Name = excelDataReader.GetValue((int)ColumnIndexes.Name).ToString()
            };
        }
    }
}
