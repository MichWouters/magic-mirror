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

        public async Task<WeatherModel> GetModelAsync(SearchCriteria criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.City)) throw new ArgumentException("A city has to be provided");

            // Get entity from Repository.
            _repo = new WeatherRepo(criteria.City);
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Map entity to model.
            WeatherModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateMappedValues(model);

            return model;
        }

        public WeatherModel CalculateMappedValues(WeatherModel model)
        {
            model.TemperatureCelsius = TemperatureHelper.KelvinToCelsius(model.TemperatureKelvin);
            model.TemperatureFahrenheit = TemperatureHelper.KelvinToFahrenheit(model.TemperatureKelvin, 0);

            DateTime sunrise = DateHelper.ConvertFromUnixTimestamp(model.SunRiseMilliseconds);
            model.SunRise = sunrise.ToString("HH:mm");

            DateTime sunset = DateHelper.ConvertFromUnixTimestamp(model.SunSetMilliSeconds);
            model.SunSet = sunset.ToString("HH:mm");
            return model;
        }

        public WeatherModel MapEntityToModel(Entity entity)
        {
            WeatherModel model = Mapper.Map<WeatherModel>(entity);
            return model;
        }
    }
}