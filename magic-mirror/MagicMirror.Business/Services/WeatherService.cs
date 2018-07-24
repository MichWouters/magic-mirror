using MagicMirror.Business.Enums;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Repos;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class WeatherService : Service<WeatherModel>, IWeatherService
    {
        private const string OFFLINEMODELNAME = "WeatherOfflineModel.json";
        private readonly IWeatherRepo _repo;

        public WeatherService(IWeatherRepo repo)
        {
            _repo = repo;
        }

        public async Task<WeatherModel> GetWeatherModel(string city)
        {
            try
            {
                // Get entity from Repository.
                var entity = await _repo.GetWeatherEntityByCityAsync(city);

                // Map entity to model.
                var model = MapFromEntity(entity);

                // Calculate non-mappable values
                model.ConvertValues();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Weather Model", ex);
            }
        }

        public WeatherModel GetOfflineModel(string path)
        {
            try
            {
                //Try reading Json object
                string json = FileWriter.ReadFromFile(path, OFFLINEMODELNAME);
                WeatherModel model = JsonConvert.DeserializeObject<WeatherModel>(json);

                return model;
            }
            catch (FileNotFoundException)
            {
                // Object does not exist. Create a new one
                WeatherModel offlineModel = GenerateOfflineModel();
                SaveOfflineModel(offlineModel, path);

                return GetOfflineModel(path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not read offline Weathermodel", e);
            }
        }

        public void SaveOfflineModel(WeatherModel model, string path)
        {
            try
            {
                string json = model.ToJson();
                FileWriter.WriteJsonToFile(json, OFFLINEMODELNAME, path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not save offline Weather Model", e);
            }
        }

        private WeatherModel GenerateOfflineModel()
        {
            return new WeatherModel
            {
                Icon = "01d",
                Location = "Mechelen",
                Sunrise = "06:44",
                Sunset = "19:42",
                Temperature = 13,
                TemperatureUom = TemperatureUom.Celsius,
                WeatherType = "Sunny"
            };
        }

        // TODO: To Presentation Layer
        private string ConvertWeatherIcon(string icon, string theme = "Dark")
        {
            try
            {
                string prefix = "ms-appx:///Assets/Weather";
                string res;

                switch (icon)
                {
                    case "01d":
                        res = "01d.png";
                        break;

                    case "01n":
                        res = "01n.png";
                        break;

                    case "02d":
                        res = "02d.png";
                        break;

                    case "02n":
                        res = "02n.png";
                        break;

                    case "03d":
                    case "03n":
                    case "04d":
                    case "04n":
                        res = "03or4.png";
                        break;

                    case "09n":
                    case "09d":
                        res = "09.png";
                        break;

                    case "10d":
                    case "10n":
                        res = "010.png";
                        break;

                    case "11d":
                        res = "11d.png";
                        break;

                    case "11n":
                        res = "11n.png";
                        break;

                    case "13d":
                    case "13n":
                        res = "13.png";
                        break;

                    case "50n":
                    case "50d":
                    default:
                        res = "50.png";
                        break;
                }
                return $"{prefix}/{theme}/{res}";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}