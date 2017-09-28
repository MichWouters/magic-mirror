using System.Threading.Tasks;
using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.Services
{
    public interface ISettingsService
    {
        string GetIpAddress();
        Task<UserSettings> LoadSettings();
        void SaveSettings();
    }
}