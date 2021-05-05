using System;
using System.Collections.Generic;
using OPTEL.Data;
using OPTEL.Data.Core;

using OPTEL.Entity.Helpers.Validation;

namespace OPTEL.Entity.Core
{
    public interface IUnitOfWork : IDisposable
    {
        #region Public repositories

        IRepository<FilmType> FilmTypeRepository { get; }

        IRepository<FilmRecipe> FilmRecipeRepository { get; }

        IRepository<Customer> CustomerRepository { get; }

        IRepository<Order> OrderRepository { get; }

        IRepository<ProductionLine> ProductionLineRepository { get; }

        IRepository<FilmTypesChange> FilmRecipeChangeRepository { get; }

        IRepository<NozzleChange> NozzleChangeRepository { get; }

        IRepository<CalibrationChange> CalibrationChangeRepository { get; }

        IRepository<CoolingLipChange> CoolingLipChangeRepository { get; }

        #endregion

        void Save();

        void SaveAsync();

        void RejectAllChanges();

        void RejectChanges<TEntity>()
            where TEntity : class, IDataObject;

        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IDataObject;

        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IDataObject;

        IEnumerable<DbEntityValidationResult> GetValidationResults();
    }
}
