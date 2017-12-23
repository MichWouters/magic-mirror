using MagicMirror.DataAccess.Entities.User;
using MagicMirror.DataAccess.Repos.Base;
using System.Collections.Generic;

namespace MagicMirror.DataAccess.Repos
{
    internal class ProfileRepository : IDataRepository<UserEntity>
    {
        public UserEntity Add(UserEntity entity)
        {
            using (var ctx = new SqliteContext())
            {
                var newEntity = ctx.Users.Add(entity);
                ctx.SaveChanges();
                return newEntity.Entity;
            }
        }

        public UserEntity Get(int id)
        {
            using (var ctx = new SqliteContext())
            {
                return ctx.Users.Find(id);
            }
        }

        public IEnumerable<UserEntity> GetAll()
        {
            using (var ctx = new SqliteContext())
            {
                return ctx.Users;
            }
        }

        public void Remove(UserEntity entity)
        {
            using (var ctx = new SqliteContext())
            {
                ctx.Users.Remove(entity);
                ctx.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var ctx = new SqliteContext())
            {
                UserEntity entity = Get(id);
                Remove(entity);
            }
        }

        public UserEntity Update(UserEntity entity)
        {
            using (var ctx = new SqliteContext())
            {
                var updateEntity = ctx.Users.Attach(entity);
                ctx.SaveChanges();
                return updateEntity.Entity;
            }
        }
    }
}