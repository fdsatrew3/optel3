using System;
using OPTEL.Data.Core;
using OPTEL.Entity.Core;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base
{
    public abstract class DatabaseExcelParcerBase<TEntity> : ExcelParcerBase<TEntity>
        where TEntity : IDataObject
    {
        protected IUnitOfWork UnitOfWork { get; }

        public DatabaseExcelParcerBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
    }
}
