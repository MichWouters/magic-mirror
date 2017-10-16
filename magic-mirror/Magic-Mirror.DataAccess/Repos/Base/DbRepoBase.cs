using MagicMirror.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public abstract class DbRepoBase<T> : IDbRepo<T> where T : class, IEntity
    {
        protected SqliteContext Context { get; private set; }

        protected DbRepoBase(SqliteContext context)
        {
            Context = context;
        }

        public Task<T> GetEntityAsync(object[] key)
        {
            return Context.Set<T>().FindAsync(key);
        }

        public Task<T> GetEntityAsync(Expression<Func<T, bool>> expression)
        {
            return Task.Run(() => Context.Set<T>().SingleOrDefault(expression));
        }

        public Task<T> InsertEntityAsync(T entity)
        {
            return Task.Run(() =>
            {
                Context.Set<T>().AddAsync(entity);
                Context.SaveChangesAsync();
                return entity;
            });
        }

        public Task<T> UpdateEntityAsync(Expression<Func<T, bool>> expression, T values)
        {
            return Task.Run(() =>
            {
                var entity = Context.Set<T>().SingleOrDefault(expression);
                if (entity != null)
                {
                    Context.Entry(entity).CurrentValues.SetValues(values);
                }
                Context.SaveChanges();
                return entity;
            });
        }

        public Task<T> DeleteEntityAsync(Expression<Func<T, bool>> expression)
        {
            return Task.Run(() =>
            {
                var entity = Context.Set<T>().SingleOrDefault(expression);
                Context.Set<T>().Remove(entity);
                Context.SaveChanges();
                return entity;
            });
        }

        public Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression)
        {
            return Task.Run(() => Context.Set<T>().Where(expression).AsEnumerable());
        }

        public Task<IEnumerable<T>> InsertEntitiesAsync(IEnumerable<T> entities)
        {
            return Task.Run(() =>
            {
                Context.Set<T>().AddRangeAsync(entities);
                Context.SaveChangesAsync();
                return entities;
            });
        }

        public Task<IEnumerable<T>> DeleteEntitiesAsync(Expression<Func<T, bool>> expression)
        {
            return Task.Run(() =>
            {
                var entities = Context.Set<T>().Where(expression).AsEnumerable();
                Context.Set<T>().RemoveRange(entities);
                Context.SaveChanges();
                return entities;
            });
        }
    }
}