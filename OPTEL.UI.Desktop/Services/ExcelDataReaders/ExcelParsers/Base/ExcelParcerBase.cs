using System;
using System.Collections.Generic;
using ExcelDataReader;
using OPTEL.Data.Core;

using OPTEL.UI.Desktop.Services.Data;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base
{
    public abstract class ExcelParcerBase<TEntity> : IExcelParcer<TEntity>
        where TEntity : IDataObject
    {
        public int StartRowIndex => _startRowIndex;

        protected abstract int WorkSheetIndex { get; }

        private const int _startRowIndex = 2;

        public IEnumerable<ActionResult<TEntity>> ParseFile(IExcelDataReader excelDataReader)
        {
            excelDataReader.Reset();

            for (int i = 1; i < WorkSheetIndex; i++)
                excelDataReader.NextResult();

            for (int i = 1; i < _startRowIndex; i++)
                excelDataReader.Read();

            while (excelDataReader.Read())
            {
                if (excelDataReader.GetValue(0) == null)
                    continue;

                var result = new ActionResult<TEntity>();

                try
                {
                    result.Result = ParseRow(excelDataReader);
                }
                catch (Exception ex)
                {
                    result.Exception = new Exception($"Parse error on the worksheet {WorkSheetIndex}", ex);
                }

                yield return result;
            }
        }

        protected abstract TEntity ParseRow(IExcelDataReader excelDataReader);
    }
}
