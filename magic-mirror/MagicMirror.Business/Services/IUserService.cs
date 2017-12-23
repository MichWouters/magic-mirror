using MagicMirror.Business.Models.User;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IUserService
    {
        Guid PersonId { get; set; }

        Task<UserProfileModel> AddUserAsync(UserProfileModel model);

        Task<UserProfileModel> GetModelAsync();

        UserProfileModel GetOfflineModel(string path);
    }
}