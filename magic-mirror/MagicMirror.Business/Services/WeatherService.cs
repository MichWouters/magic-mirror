using Acme.Generic;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class WeatherService : ServiceBase<WeatherModel, WeatherEntity>
    {
        public WeatherService(UserSettings criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.HomeCity)) throw new ArgumentException("A city has to be provided");

            _criteria = criteria;
        }

        public override async Task<WeatherModel> GetModelAsync()
        {
            // Get entity from Repository.
            WeatherEntity entity = await GetEntityAsync();

            // Map entity to model.
            WeatherModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateUnMappableValues(model);

            // Todo: Implement bool if user wants metro or openweather icons
            if (true)
            {
                model.Icon = ConvertWeatherIcon(model.Icon);
            }

            return model;
        }

        protected override async Task<WeatherEntity> GetEntityAsync()
        {
            var repo = new WeatherRepo(_criteria.HomeCity);
            WeatherEntity entity = await repo.GetEntityAsync();

            return entity;
        }

        protected override WeatherModel CalculateUnMappableValues(WeatherModel model)
        {
            model.TemperatureCelsius = TemperatureHelper.KelvinToCelsius(model.TemperatureKelvin, _criteria.Precision);
            model.TemperatureFahrenheit = TemperatureHelper.KelvinToFahrenheit(model.TemperatureKelvin, _criteria.Precision);

            DateTime sunrise = DateHelper.ConvertFromUnixTimestamp(model.SunRiseMilliseconds);
            DateTime sunset = DateHelper.ConvertFromUnixTimestamp(model.SunSetMilliSeconds);
            model.SunRise = sunrise.ToString("HH:mm");
            model.SunSet = sunset.ToString("HH:mm");

            return model;
        }

        /// <summary>
        /// Convert an OpenWeatherMap icon to a Metro Style weather icon
        /// </summary>
        /// <param name="icon">OpenweatherMap icon to convert</param>
        /// <param name="theme">The colour scheme. Choice between light and dark</param>
        /// <returns></returns>
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
                //return $"{prefix}/{theme}/{res}";
                return "ms - appx:///Assets/Weather/Dark/010.png";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}