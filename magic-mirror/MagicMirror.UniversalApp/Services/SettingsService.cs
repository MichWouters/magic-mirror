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
            UserSettings settings = new UserSettings
            {
                DistanceUOM = DistanceUOM.Metric,
                HomeAddress = "Heikant 51",
                HomeCity = "Houwaart",
                Precision = 2,
                TemperatureUOM = TemperatureUOM.Celsius,
                UserName = "Michiel",
                WorkAddress = "Generaal Armstrongweg 1, Antwerpen"
            };

            string json = JsonConvert.SerializeObject(settings);

            await SaveJsonToLocalSettings(json);
        }

        private async Task SaveJsonToLocalSettings(string json)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync("userSettings.json", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file,json);
        }
    }
}
