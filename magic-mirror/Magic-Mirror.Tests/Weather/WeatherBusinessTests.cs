using MagicMirror.Business.Models.Weather;
using MagicMirror.Business.Services;
using MagicMirror.Business.Services.Weather;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.Weather;
using MagicMirror.DataAccess.Weather;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherBussinessTests
    {
        private IRepo<WeatherEntity> _repo;
        private IService<WeatherModel> _service;

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
        public async Task Mappings_Are_Correct()
        {
            // Arrange
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Act
            WeatherModel model = _service.MapEntityToModel(entity);
            model = _service.CalculateMappedValues(model);

            // Assert
            Assert.Equal(entity.Weather[0].Description, model.Description);
            Assert.Equal(entity.Weather[0].Icon, model.Icon);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Weather[0].Main, model.WeatherType);
            Assert.InRange(model.DegreesCelsius, -20, 50);
        }

        [Fact]
        public async Task Can_Map_Entity_To_Model()
        {
            // Arrange
            WeatherEntity entity = await _repo.GetEntityAsync();

            // Act
            WeatherModel model = _service.MapEntityToModel(entity);
            model = _service.CalculateMappedValues(model);

            // Assert
            Assert.NotNull(model);
            Assert.NotEqual(0, model.DegreesKelvin);
            


            // Todo: Show difference in equality for reference types
            // E.G: var ref1 = new obj, var ref2 = ref1 etc...
            // new ref1 with same values as ref2 != equality!
            Assert.NotEqual("", model.Description);
            Assert.NotEqual("", model.Icon);
            Assert.NotEqual("", model.SunRise);
            Assert.NotEqual("", model.SunSet);
        }
    }
}