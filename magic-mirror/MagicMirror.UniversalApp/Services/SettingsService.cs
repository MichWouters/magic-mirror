using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
using System.IO;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService : ISettingsService
    {
        private CommonService _commonService;
        private const string USERSETTINGS = "userSettings.json";

        public SettingsService()
        {
            _commonService = new CommonService();
        }

        public UserSettings LoadSettings()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var result = _commonService.ReadSettings(localFolder.Path, USERSETTINGS);
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
            var localFolder = ApplicationData.Current.LocalFolder;
            _commonService.SaveSettings(localFolder.Path, USERSETTINGS, settings);
        }
    }
}