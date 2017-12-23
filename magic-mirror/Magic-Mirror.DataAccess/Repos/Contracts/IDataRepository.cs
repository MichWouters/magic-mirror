using MagicMirror.DataAccess.Entities.Entities;
using System.Collections.Generic;

namespace MagicMirror.DataAccess.Repos.Base
{
    public interface IDataRepository
    {
    }

    public interface IDataRepository<T> : IDataRepository
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);

        void Remove(T entity);

        void Remove(int id);

        T Update(T entity);

        IEnumerable<T> GetAll();

        T Get(int id);
    }
}