using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OPTEL.Data;
using OPTEL.Data.Core;

using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.Entity.Helpers.Exceptions;
using OPTEL.Entity.Helpers.Validation;

namespace OPTEL.Entity
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(IDataBaseEnsurer ensurer)
        {
            ensurer.Ensure(Database);
        }

        #region Public methods

        public void RejectAllChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void RejectChanges<TEntity>()
            where TEntity : class, IDataObject
        {
            foreach (var entry in ChangeTracker.Entries<TEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void ValidateContext()
        {
            var result = GetValidationErrors().ToList();

            if (result.Count > 0)
                throw new DbEntityValidationException(result);
        }

        public IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            var entries = ChangeTracker.Entries()
                .Where(s => s.State == EntityState.Added || s.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(entry.Entity);

                if (!Validator.TryValidateObject(entry.Entity, validationContext, validationResults, true))
                {
                    yield return new DbEntityValidationResult(entry, validationResults);
                }
            }
        }


        #endregion

        #region DbSets

        public DbSet<FilmType> FilmTypes { get; set; }

        public DbSet<FilmRecipe> FilmRecipes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductionLine> ProductionLines { get; set; }

        public DbSet<FilmTypesChange> FilmRecipeChanges { get; set; }

        public DbSet<NozzleChange> NozzleChanges { get; set; }

        public DbSet<CalibrationChange> CalibrationChanges { get; set; }

        public DbSet<CoolingLipChange> CoolingLipChanges { get; set; }

        #endregion

        #region Overrides of DbContext

        public override int SaveChanges()
        {
            ValidateContext();

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            EnsureDirectoryCreated("Databases");
            optionsBuilder.UseSqlite(@$"Data Source={Path.Combine(Environment.CurrentDirectory, "Databases", "DungeonsDatabase.db")}");

#if LogConsole || LogFile
            optionsBuilder.LogTo(Log);
#endif

            base.OnConfiguring(optionsBuilder);
        }

        private void Log(string message)
        {
#if LogConsole
            Debug.WriteLine(message);
#endif
#if LogFile
            string filename = $"logs\\{DateTime.Today}.log";
            File.AppendAllText(filename, Environment.NewLine + message);
#endif
        }

        private void EnsureDirectoryCreated(string dirName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, dirName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path).Create();
        }

        #endregion
    }
}
