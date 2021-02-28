using System.Data.Entity;

using OPTEL.Data;

namespace OPTEL.Entity
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DefaultConnection")
        {
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
            where TEntity : class, Data.Core.IDataObject
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


        #endregion

        #region DbSets

        public DbSet<FilmType> FilmTypes { get; set; }

        public DbSet<FilmRecipe> FilmRecipes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Extruder> Extruders { get; set; }

        public DbSet<FilmRecipeChange> FilmRecipeChanges { get; set; }

        public DbSet<NozzleChange> NozzleChanges { get; set; }

        public DbSet<CalibrationChange> CalibrationChanges { get; set; }

        public DbSet<CoolingLipChange> CoolingLipChanges { get; set; }

        #endregion

        #region Overrides of DbContext

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if  LogConsole || LogFile
            Database.Log += Log;
#endif
        }

        #endregion
    }
}
