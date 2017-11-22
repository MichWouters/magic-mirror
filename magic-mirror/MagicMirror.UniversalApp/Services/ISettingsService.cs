using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.Services
{
    public interface ISettingsService
    {
        UserSettings LoadSettings();

        void SaveSettings(UserSettings userSettings, bool createNewSettings = false)
    }
}