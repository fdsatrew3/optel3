using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OPTEL.Data;
using OPTEL.Data.Core;
using OPTEL.Data.Users;
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

        #region Users

        IRepository<User> UserRepository { get; }

        IRepository<Administrator> AdministratorRepository { get; }

        IRepository<ProductionDirector> ProductionDirectorRepository { get; }

        IRepository<KnowledgeEngineer> KnowledgeEngineerRepository { get; }

        #endregion

        #endregion

        void Save();

        Task SaveAsync();

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
