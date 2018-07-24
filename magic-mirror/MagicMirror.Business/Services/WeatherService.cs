using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Repos;
using System;
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
                var entity = await _repo.GetWeatherEntityByCityAsync(city);
                var model = MapFromEntity(entity);
                model.ConvertValues();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Weather Model", ex);
            }
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