using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.Services
{
    public interface ILocationService
    {
        Task GetLocationAsync();
    }
}