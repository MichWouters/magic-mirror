using MagicMirror.DataAccess.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class UserRepo : DbRepoBase<UserEntity>
    {
        public UserRepo(SqliteContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetUserByFaceIdAsync(Guid faceId)
        {
            return await GetEntityAsync(x => x.FaceId == faceId);
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            return await InsertEntityAsync(user);
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity user)
        {
            return await UpdateEntityAsync(x => x.Id == user.Id, user);
        }

        public async Task<UserEntity> DeleteUserAsync(UserEntity user)
        {
            return await DeleteEntityAsync(x => x.Id == user.Id);
        }
    }
}
