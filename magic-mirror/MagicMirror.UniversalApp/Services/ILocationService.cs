using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MagicMirror.UniversalApp.Services
{
    public interface ILocationService
    {
        Task<Geoposition> GetLocationAsync();
    }
}