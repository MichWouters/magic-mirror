using System.Threading.Tasks;
using MagicMirror.Business.Models;

namespace MagicMirror.Business.Services
{
    public interface IAddressService
    {
        Task<AddressModel> GetModelAsync(string latitude, string longitude);
    }
}