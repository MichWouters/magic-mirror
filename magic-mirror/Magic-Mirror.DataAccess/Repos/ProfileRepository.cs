using MagicMirror.DataAccess.Entities.User;
using MagicMirror.DataAccess.Repos.Base;
using System.Collections.Generic;

namespace MagicMirror.DataAccess.Repos
{
    internal class ProfileRepository : IDataRepository<UserEntity>
    {
        public UserEntity Add(UserEntity entity)
        {
            using (var ctx = new MirrorContext())
            {
                var newEntity = ctx.Users.Add(entity);
                ctx.SaveChanges();
                return newEntity.Entity;
            }
        }

        public UserEntity Get(int id)
        {
            using (var ctx = new MirrorContext())
            {
                return ctx.Users.Find(id);
            }
        }

        public IEnumerable<UserEntity> GetAll()
        {
            using (var ctx = new MirrorContext())
            {
                return ctx.Users;
            }
        }

        public void Remove(UserEntity entity)
        {
            using (var ctx = new MirrorContext())
            {
                ctx.Users.Remove(entity);
                ctx.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var ctx = new MirrorContext())
            {
                UserEntity entity = Get(id);
                Remove(entity);
            }
        }

        public UserEntity Update(UserEntity entity)
        {
            using (var ctx = new MirrorContext())
            {
                var updateEntity = ctx.Users.Attach(entity);
                ctx.SaveChanges();
                return updateEntity.Entity;
            }
        }
    }
}