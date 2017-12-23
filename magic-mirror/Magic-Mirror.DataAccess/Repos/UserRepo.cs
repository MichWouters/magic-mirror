using MagicMirror.DataAccess.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class UserRepo : DbRepo<UserEntity>
    {
        public UserRepo(MirrorContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetUserByPersonId(Guid personId)
        {
            return await GetEntityAsync(x => x.PersonId == personId);
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            foreach (var a in user.Addresses)
            {
                a.AddressType = Context.AddressTypes.Find(a.AddressType.Id);
            }
            return await InsertEntityAsync(user);
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity user)
        {
            foreach (var a in user.Addresses)
            {
                a.AddressType = Context.AddressTypes.Find(a.AddressType.Id);
            }
            return await UpdateEntityAsync(x => x.Id == user.Id, user);
        }

        public async Task<UserEntity> DeleteUserAsync(UserEntity user)
        {
            return await DeleteEntityAsync(x => x.Id == user.Id);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return Context.Users.AsEnumerable();
        }
    }
}