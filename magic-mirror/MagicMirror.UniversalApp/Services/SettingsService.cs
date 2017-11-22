using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;
using Windows.Storage;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService : ISettingsService
    {
        private Business.Services.SettingsService _settingsService;
        private readonly string localFolder = ApplicationData.Current.LocalFolder.Path;
        private const string SETTING_FILE = "settings.json";

        public SettingsService()
        {
            _settingsService = new Business.Services.SettingsService();
        }

        public void SaveSettings(UserSettings userSettings, bool createNewSettings = false)
        {
            try
            {
                if (createNewSettings || userSettings == null)
                {
                    userSettings = new UserSettings();
                    //DisplayErrorMessage("Default settings created", "No settings file found! Creating a new one with default settings");
                }

                if (userSettings != null)
                {
                    _settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
                    App.NavigationService.Navigate<MainPage>();
                }
                else
                {
                    throw new ArgumentException("Unable to save Settings. Please check your input");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to save Settings. " + e.Message.ToString(), e);
            }
        }

        public UserSettings LoadSettings()
        {
            try
            {
                var result = _settingsService.ReadSettings(localFolder, SETTING_FILE);

                if (result == null) throw new FileNotFoundException();

                return result;
            }
            catch (FileNotFoundException)
            {
                SaveSettings(null, true);
                return LoadSettings();
                throw;
            }
            catch (Exception) { throw; }
        }
    }
}