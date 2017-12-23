using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IAddressService
    {
        Task<AddressModel> GetModelAsync(string latitude, string longitude);
    }
}