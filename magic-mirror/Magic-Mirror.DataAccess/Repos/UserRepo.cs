using MagicMirror.DataAccess.Entities.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class UserRepo : DbRepoBase<UserEntity>
    {
        public UserRepo(SqliteContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetUserByPersonId(Guid personId)
        {
            return await GetEntityAsync(x => x.PersonId == personId);
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            return await InsertEntityAsync(user);
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity user)
        {
            foreach (var ua in user.Addresses)
            {
                Context.Set<UserAddres>().Attach(ua);
                Context.Set<Address>().Attach(ua.Address);
            }
            foreach (var f in user.Faces)
            {
                Context.Set<UserFace>().Attach(f);
            }
            return await UpdateEntityAsync(x => x.Id == user.Id, user);
        }

        public async Task<UserEntity> DeleteUserAsync(UserEntity user)
        {
            return await DeleteEntityAsync(x => x.Id == user.Id);
        }
    }
}