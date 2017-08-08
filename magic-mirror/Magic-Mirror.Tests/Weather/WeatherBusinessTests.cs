using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.Weather;
using System.Threading.Tasks;
using MagicMirror.DataAccess.Repos;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherBussinessTests
    {
        private readonly IRepo<WeatherEntity> _repo;
        private readonly IService<WeatherModel> _service;

        public WeatherBussinessTests()
        {
            SearchCriteria criteria = new SearchCriteria
            {
                City = "London"
            };

            _repo = new WeatherRepo(criteria);
            _service = new WeatherService();
        }

        [Fact]
        public async Task Can_Map_Entity_To_Model()
        {
            // Arrange
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Act
            WeatherModel model = _service.MapEntityToModel(entity);

            // Assert
            // Todo: Show difference in equality for reference types
            // E.G: var ref1 = new obj, var ref2 = ref1 etc...
            // new ref1 with same values as ref2 != equality!
            Assert.NotNull(model);

        }

        [Fact]
        public async Task AutoMapped_Values_Correct()
        {
            // Arrange
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Act
            WeatherModel model = _service.MapEntityToModel(entity);

            // Assert
            Assert.NotEqual(0, model.TemperatureKelvin);
            Assert.NotEqual("", model.Description);
            Assert.NotEqual("", model.Icon);
            Assert.NotEqual("", model.Name);
            Assert.NotEqual(0, model.SunRiseMilliseconds);
            Assert.NotEqual(0, model.SunSetMilliSeconds);
            Assert.Equal(entity.Weather[0].Icon, model.Icon);
            Assert.Equal(entity.Weather[0].Main, model.WeatherType);
        }

        [Fact]
        public void Calculated_Fields_Correct()
        {
            // Arrange
            WeatherModel model = new WeatherModel
            {
                SunRiseMilliseconds = 0,
                SunSetMilliSeconds = 0,
                TemperatureKelvin = 100,
            };

            // Act
            model = _service.CalculateMappedValues(model);

            // Assert
            // Todo: Add additional checks
            Assert.InRange(model.TemperatureCelsius, -20, 50);
            Assert.NotEqual(0, model.TemperatureFahrenheit);
            Assert.NotEqual(0, model.TemperatureKelvin);
        }
    }
}