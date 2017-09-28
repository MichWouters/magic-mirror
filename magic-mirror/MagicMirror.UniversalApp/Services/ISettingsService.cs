using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.Services
{
    public interface ISettingsService
    {
        string GetIpAddress();

        UserSettings LoadSettings();

        void SaveSettings(UserSettings settings);
    }
}