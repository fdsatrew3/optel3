using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OPTEL.Entity.Helpers.Ensurers
{
    public class ProductionDataBaseEnsurer : IDataBaseEnsurer
    {
        public void Ensure(DatabaseFacade database)
        {
            database.EnsureCreated();
        }
    }
}
