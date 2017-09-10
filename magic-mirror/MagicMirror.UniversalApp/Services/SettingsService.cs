using MagicMirror.Business.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MagicMirror.UniversalApp.Services
{
    public class SettingsService
    {
        public void ReadSettings()
        {
            throw new NotImplementedException();
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
            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(sampleFile,json);
        }
    }
}
