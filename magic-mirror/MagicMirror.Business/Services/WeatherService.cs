using Acme.Generic;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Entities.Weather;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class WeatherService : ServiceBase, IService<WeatherModel>
    {
        private IRepo<WeatherEntity> _repo;
        private SearchCriteria _criteria;

        public WeatherService(SearchCriteria criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.HomeCity)) throw new ArgumentException("A city has to be provided");

            _criteria = criteria;
        }

        public async Task<WeatherModel> GetModelAsync()
        {
            // Get entity from Repository.
            _repo = new WeatherRepo(_criteria.HomeCity);
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Map entity to model.
            WeatherModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateMappedValues(model);

            return model;
        }

        public WeatherModel CalculateMappedValues(WeatherModel model)
        {
            switch (_criteria.TemperatureUOM)
            {
                case TemperatureUOM.Celsius:
                    model.TemperatureCelsius = TemperatureHelper.KelvinToCelsius(model.TemperatureKelvin, _criteria.Precision);
                    break;

                case TemperatureUOM.Fahrenheit:
                    model.TemperatureFahrenheit = TemperatureHelper.KelvinToFahrenheit(model.TemperatureKelvin, _criteria.Precision);
                    break;

                default:
                    break;
            }

            DateTime sunrise = DateHelper.ConvertFromUnixTimestamp(model.SunRiseMilliseconds);
            model.SunRise = sunrise.ToString("HH:mm");

            DateTime sunset = DateHelper.ConvertFromUnixTimestamp(model.SunSetMilliSeconds);
            model.SunSet = sunset.ToString("HH:mm");
            return model;
        }

        private WeatherModel MapEntityToModel(Entity entity)
        {
            WeatherModel model = Mapper.Map<WeatherModel>(entity);
            return model;
        }
    }
}