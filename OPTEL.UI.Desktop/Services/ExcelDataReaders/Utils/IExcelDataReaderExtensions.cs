using ExcelDataReader;
using ExcelNumberFormat;
using System.Globalization;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils
{
    public static class IExcelDataReaderExtensions
    {
        public static string GetFormattedValue(this IExcelDataReader excelDataReader, int columnIndex)
        {
            var value = excelDataReader.GetValue(columnIndex);
            var formatString = excelDataReader.GetNumberFormatString(columnIndex);

            if (formatString != null)
            {
                var format = new NumberFormat(formatString);
                return format.Format(value, CultureInfo.CurrentCulture);
            }
            if (value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }

        /// <summary>
        /// Read string value from cell by column index
        /// </summary>
        /// <param name="excelDataReader">Extension parameter</param>
        /// <param name="columnIndex">Should ba able to convert to int</param>
        /// <returns>String value</returns>
        public static string GetFormattedValue(this IExcelDataReader excelDataReader, object columnIndex)
        {
            return excelDataReader.GetFormattedValue((int)columnIndex);
        }

        public static object GetValue(this IExcelDataReader excelDataReader, object columnIndex)
        {
            return excelDataReader.GetValue((int)columnIndex);
        }

        public static string GetString(this IExcelDataReader excelDataReader, object columnIndex)
        {
            return excelDataReader.GetString((int)columnIndex);
        }
    }
}
