using System.Threading.Tasks;
using MagicMirror.Business.Models;

namespace MagicMirror.Business.Services
{
    public interface IAddressService
    {
        Task<AddressModel> GetModelAsync();
        AddressModel GetOfflineModel(string path);
        void SaveOfflineModel(AddressModel model, string path);
    }
}