using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using OPTEL.Entity.Core;

namespace OPTEL.Entity.Persistance
{
    public class EFRepository<TEntity, TContext> : EFReadonlyRepository<TEntity, TContext>, IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class, Data.Core.IDataObject
    {
        public EFRepository(TContext context) :
            base(context)
        {
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            var set = _context.Set<TEntity>();
            if (_context.Entry(entity).State == EntityState.Detached)
                set.Attach(entity);

            set.Remove(entity);
        }

        public virtual void Delete(Guid id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            var ids = entities.Select(entity => entity.ID).ToList();
            DeleteRange(ids);
        }

        public virtual void DeleteRange(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        public virtual void SetEntityEntryStateModified(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetEntityEntryStateUnmodified(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Unchanged;
        }
    }
}
