using Acme.Generic;
using MagicMirror.Business.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService
    {
        public async Task<UserSettings> ReadSettings()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync("userSettings.json");

            string json = await FileIO.ReadTextAsync(file);
            var result = JsonConvert.DeserializeObject<UserSettings>(json);

            return result;
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
