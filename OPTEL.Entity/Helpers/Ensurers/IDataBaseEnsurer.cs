using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OPTEL.Entity.Helpers.Ensurers
{
    public interface IDataBaseEnsurer
    {
        void Ensure(DatabaseFacade database);
    }
}
