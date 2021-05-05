using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using OPTEL.Data;
using OPTEL.Data.Core;
using OPTEL.Entity.Core;
using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.Entity.Helpers.Exceptions;
using OPTEL.Entity.Helpers.Validation;

namespace OPTEL.Entity.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDataBaseEnsurer ensurer)
        {
            _context = new DatabaseContext(ensurer);
        }

        #region Private members

        private readonly DatabaseContext _context;

        private IRepository<FilmType> _filmTypeRepository;
        private IRepository<FilmRecipe> _filmRecipeRepository;
        private IRepository<Customer> _customerRepository;
        private IRepository<Order> _orderRepository;
        private IRepository<ProductionLine> _productionLineRepository;
        private IRepository<FilmTypesChange> _filmRecipeChangeRepository;
        private IRepository<NozzleChange> _nozzleChangeRepository;
        private IRepository<CalibrationChange> _calibrationChangeRepository;
        private IRepository<CoolingLipChange> _coolingLipChangeRepository;

        #endregion

        #region Public members

        public IRepository<FilmType> FilmTypeRepository => _filmTypeRepository ??= new EFRepository<FilmType, DatabaseContext>(_context);

        public IRepository<FilmRecipe> FilmRecipeRepository =>_filmRecipeRepository ??= new EFRepository<FilmRecipe, DatabaseContext>(_context);

        public IRepository<Customer> CustomerRepository => _customerRepository ??= new EFRepository<Customer, DatabaseContext>(_context);

        public IRepository<Order> OrderRepository => _orderRepository ??= new EFRepository<Order, DatabaseContext>(_context);

        public IRepository<ProductionLine> ProductionLineRepository => _productionLineRepository ??= new EFRepository<ProductionLine, DatabaseContext>(_context);

        public IRepository<FilmTypesChange> FilmRecipeChangeRepository => _filmRecipeChangeRepository ??= new EFRepository<FilmTypesChange, DatabaseContext>(_context);

        public IRepository<NozzleChange> NozzleChangeRepository => _nozzleChangeRepository ??= new EFRepository<NozzleChange, DatabaseContext>(_context);

        public IRepository<CalibrationChange> CalibrationChangeRepository => _calibrationChangeRepository ??= new EFRepository<CalibrationChange, DatabaseContext>(_context);

        public IRepository<CoolingLipChange> CoolingLipChangeRepository => _coolingLipChangeRepository ??= new EFRepository<CoolingLipChange, DatabaseContext>(_context);

        #endregion

        protected virtual void ThrowUpdatedValidationException(DbEntityValidationException exception)
        {
            var errorMessages =
                exception.EntityValidationResults
                    .SelectMany(x => x.ValidationResults)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join($"; {Environment.NewLine}", errorMessages);
            var exceptionMessage = string.Concat(exception.Message, " The validation errors are: ", fullErrorMessage);

            //Запихаем в InnerException источник ошибок валидации
            var invalidEntities = exception.EntityValidationResults.Select(item => item.Entry.Entity.GetType().Name).Distinct();
            var innerExceptionMessage = string.Concat("Invalid Entities are:",
                string.Join($"; {Environment.NewLine}", invalidEntities));

            throw new DbEntityValidationException(exceptionMessage, exception.EntityValidationResults, new Exception(innerExceptionMessage));
        }

        public void RejectAllChanges() => _context.RejectAllChanges();

        public void RejectChanges<TEntity>()
            where TEntity : class, IDataObject => _context.RejectChanges<TEntity>();

        public void Save()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                ThrowUpdatedValidationException(ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _context.RejectAllChanges();
                throw;
            }
        }

        public async void SaveAsync()
        {
            await Task.Run(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    await transaction.RollbackAsync();
                    ThrowUpdatedValidationException(ex);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    _context.RejectAllChanges();
                    throw;
                }
            });
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class, IDataObject
        {
            var repos = (from property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         let generics = property.PropertyType.GetGenericArguments()
                         where generics.Length == 1 && generics.First() == typeof(TEntity)
                         select property.GetValue(this)).ToList();

            if (repos.Count == 0)
                throw new ArgumentException($"No read-only repository of type {typeof(TEntity).Name} is defined");
            if (repos.Count > 1)
                throw new ArgumentException($"Multiple read-only repositories of type {typeof(TEntity).Namespace} are defined");
            if (!(repos.First() is IReadOnlyRepository<TEntity> repo))
                throw new ArgumentException($"The repository of type {typeof(TEntity).Name} can not be casted to needed type");

            return repo;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IDataObject
        {
            var repos = (from property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         let generics = property.PropertyType.GetGenericArguments()
                         where generics.Length == 1 && generics.First() == typeof(TEntity)
                         select property.GetValue(this)).ToList();

            if (repos.Count == 0)
                throw new ArgumentException($"No repository of type {typeof(TEntity).Name} is defined");
            if (repos.Count > 1)
                throw new ArgumentException($"Multiple repositories of type {typeof(TEntity).Namespace} are defined");
            if (!(repos.First() is IRepository<TEntity> repo))
                throw new ArgumentException($"The repository of type {typeof(TEntity).Name} can not be casted to needed type");

            return repo;
        }


        public IEnumerable<DbEntityValidationResult> GetValidationResults()
        {
            return _context.GetValidationErrors();
        }

        #region IDisposable

        public void Dispose()
        {
            _context?.Dispose();
        }

        #endregion
    }
}
