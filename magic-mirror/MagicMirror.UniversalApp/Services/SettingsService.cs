using MagicMirror.Business.Models;
using System;
using System.IO;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService : ISettingsService
    {
        private Business.Services.ISettingsService settingsService;
        private const string USERSETTINGS = "userSettings.json";
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public SettingsService()
        {
            settingsService = new Business.Services.SettingsService();
        }

        public UserSettings LoadSettings()
        {
            try
            {
                var result = settingsService.ReadSettings(localFolder.Path, USERSETTINGS);
                return result;
            }
            catch (FileNotFoundException)
            {
                SaveSettings(new UserSettings());
                throw;
            }
        }

        public string GetIpAddress()
        {
            try
            {
                string result = "";
                foreach (Windows.Networking.HostName localHostName in NetworkInformation.GetHostNames())
                {
                    if (localHostName.IPInformation != null)
                    {
                        if (localHostName.Type == Windows.Networking.HostNameType.Ipv4)
                        {
                            result = localHostName.ToString();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(result)) throw new Exception();
                return result;
            }
            catch (Exception)
            {
                return "Unable to retrieve IP Address";
            }
        }

        public void SaveSettings(UserSettings settings)
        {
            settingsService.SaveSettings(localFolder.Path, USERSETTINGS, settings);
        }
    }
}