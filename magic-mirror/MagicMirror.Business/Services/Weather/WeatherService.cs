using MagicMirror.Business.Models.Weather;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.Weather;
using System.Threading.Tasks;
using MagicMirror.DataAccess.Entities;

namespace MagicMirror.Business.Services.Weather
{
    public class WeatherService : ServiceBase, IService<WeatherModel>
    {
        private readonly IRepo<WeatherEntity> _repo;
        private SearchCriteria _criteria;

        public WeatherService()
        {
            _criteria = new SearchCriteria
            {
                City = "London"
            };

            _repo = new WeatherRepo(_criteria);
        }

        public WeatherModel CalculateMappedValues(WeatherModel model)
        {
            model.DegreesCelsius = Helpers.TemperatureHelper.KelvinToCelsius(model.DegreesKelvin, 1);
            model.DegreesFahrenheit = Helpers.TemperatureHelper.KelvinToFahrenheit(model.DegreesKelvin, 0);
            return model;
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

        public WeatherModel MapEntityToModel(Entity entity)
        {
            WeatherModel model = _mapper.Map<WeatherModel>(entity);
            return model;
        }
    }
}
