using MagicMirror.Business.Models.User;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.User;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class UserService : ServiceBase<UserProfileModel, UserEntity>, IUserService
    {
        private UserRepo _userRepo;

        public UserService(SqliteContext context)
        {
            _userRepo = new UserRepo(context);
        }

        public Guid PersonId { get; set; }

        public async Task<UserProfileModel> AddUserAsync(UserProfileModel model)
        {
            UserEntity entity = MapModelToEntity(model);
            entity = await _userRepo.AddUserAsync(entity);
            return MapEntityToModel(entity);
        }

        public async Task<UserProfileModel> GetModelAsync()
        {
            UserEntity entity = await _userRepo.GetUserByPersonId(PersonId);
            UserProfileModel model = MapEntityToModel(entity);
            model = CalculateUnMappableValues(model);
            return model;
        }

        public UserProfileModel GetOfflineModel(string path)
        {
            return (new UserProfileModel()).RandomData("male");
        }

        protected UserProfileModel CalculateUnMappableValues(UserProfileModel model)
        {
            return model;
        }
    }
}