using System;
using System.Collections.Generic;

namespace OPTEL.Entity.Core
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, Data.Core.IDataObject
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(Guid id);

        void DeleteRange(IEnumerable<TEntity> entities);

        void DeleteRange(IEnumerable<Guid> ids);

        void SetEntityEntryStateModified(TEntity entity);

        void SetEntityEntryStateUnmodified(TEntity entity);
    }
}
