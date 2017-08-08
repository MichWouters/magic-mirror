using System;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Entities.Weather;
using System.Threading.Tasks;
using Acme.Generic;
using MagicMirror.DataAccess.Repos;

namespace MagicMirror.Business.Services
{
    public class WeatherService : ServiceBase, IService<WeatherModel>
    {
        private readonly IRepo<WeatherEntity> _repo;

        public WeatherService()
        {
            var criteria = new SearchCriteria
            {
                City = "London"
            };

            _repo = new WeatherRepo(criteria);
        }

        public async Task<WeatherModel> GetModelAsync()
        {
            // Get entity from Repository.
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Map entity to model.
            WeatherModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateMappedValues(model);

            return model;
        }

        public WeatherModel CalculateMappedValues(WeatherModel model)
        {
            model.TemperatureCelsius = TemperatureHelper.KelvinToCelsius(model.TemperatureKelvin, 1);
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