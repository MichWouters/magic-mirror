using Acme.Generic;
using MagicMirror.Business.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Networking.Connectivity;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService : ISettingsService
    {
        public async Task<UserSettings> ReadSettings()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync("userSettings.json");

            string json = await FileIO.ReadTextAsync(file);
            var result = JsonConvert.DeserializeObject<UserSettings>(json);

            return result;
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

        public async Task SaveSettings()
        {
            var settings = GetUserSettings();

            string json = JsonConvert.SerializeObject(settings);
            var localFolder = ApplicationData.Current.LocalFolder;

            FileWriter.WriteJsonToFile(json, "settings.json", localFolder.Path);
        }

        private UserSettings GetUserSettings()
        {
            var settings = new UserSettings
            {
                DistanceUOM = DistanceUOM.Metric,
                HomeAddress = "Heikant 51",
                HomeCity = "Houwaart",
                Precision = 2,
                TemperatureUOM = TemperatureUOM.Celsius,
                UserName = "Michiel",
                WorkAddress = "Heerlen, Netherlands"
            };

            return settings;
        }
    }
}
