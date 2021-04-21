using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OPTEL.Entity.Helpers.Ensurers
{
    public class TestingDataBaseEnsurer : IDataBaseEnsurer
    {
        public void Ensure(DatabaseFacade database)
        {
            database.EnsureDeleted();
            database.EnsureCreated();
        }
    }
}
