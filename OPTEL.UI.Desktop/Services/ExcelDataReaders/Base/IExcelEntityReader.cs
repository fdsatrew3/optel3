using System.Collections.Generic;
using OPTEL.UI.Desktop.Services.Data;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.Base
{
    public interface IExcelEntityReader
    {
        IEnumerable<ActionResult> ReadFile(string filePath);
    }
}
