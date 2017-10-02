using MagicMirror.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public interface IDbRepo<T> where T : IEntity
    {
        Task<T> GetEntityAsync(object[] key);
        Task<T> GetEntityAsync(Expression<Func<T, bool>> expression);
        Task<T> InsertEntityAsync(T entity);
        Task<T> UpdateEntityAsync(Expression<Func<T, bool>> expression, T values);
        Task<T> DeleteEntityAsync(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> InsertEntitiesAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> DeleteEntitiesAsync(Expression<Func<T, bool>> expression);
    }
}