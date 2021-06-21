using System.Collections.Generic;
using ExcelDataReader;

using OPTEL.UI.Desktop.Services.Data;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base
{
    public interface IExcelParcer<T>
    {
        int StartRowIndex { get; }

        IEnumerable<ActionResult<T>> ParseFile(IExcelDataReader excelDataReader);
    }
}
