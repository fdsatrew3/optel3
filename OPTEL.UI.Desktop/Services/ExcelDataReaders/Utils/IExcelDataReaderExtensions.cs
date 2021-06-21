using System;
using System.Globalization;
using ExcelDataReader;
using ExcelNumberFormat;

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
        public static string GetFormattedValue(this IExcelDataReader excelDataReader, object columnIndex) => excelDataReader.GetFormattedValue((int)columnIndex);

        public static object GetValue(this IExcelDataReader excelDataReader, object columnIndex) => excelDataReader.GetValue((int)columnIndex);

        public static string GetString(this IExcelDataReader excelDataReader, object columnIndex)
        {
            var result = excelDataReader.GetValue((int)columnIndex);

            if (result is null)
                return string.Empty;
            else
                return result.ToString();
        }

        public static double GetDouble(this IExcelDataReader excelDataReader, object columnIndex) => Convert.ToDouble(excelDataReader.GetValue(columnIndex));

        public static double? GetNullabeDouble(this IExcelDataReader excelDataReader, object columnIndex)
        {
            var value = excelDataReader.GetValue(columnIndex);

            if (value == null)
                return null;
            else
                return Convert.ToDouble(value);
        }
    }
}
