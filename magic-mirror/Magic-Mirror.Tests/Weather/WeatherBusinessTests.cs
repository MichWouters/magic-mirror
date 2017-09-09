using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherBussinessTests
    {
        private readonly IApiService<WeatherModel> _service;
        private SearchCriteria criteria;

        public WeatherBussinessTests()
        {
            criteria = new SearchCriteria
            {
                HomeCity = "London",
                Precision = 1,
            };

            _service = new WeatherService(criteria);
        }

        [Fact]
        public async Task AutoMapped_Fields_Correct()
        {
            // Arrange
            WeatherModel model = await _service.GetModelAsync();

            // Assert
            Assert.NotEqual(0, model.TemperatureKelvin);
            Assert.NotEqual("", model.Description);
            Assert.NotEqual("", model.Icon);
            Assert.NotEqual("", model.Name);
            Assert.NotEqual(0, model.SunRiseMilliseconds);
            Assert.NotEqual(0, model.SunSetMilliSeconds);
        }

        [Fact]
        public void Calculated_Fields_Correct()
        {
            //// Arrange
            //WeatherModel model = new WeatherModel
            //{
            //    SunRiseMilliseconds = 1502426469,
            //    SunSetMilliSeconds = 1502479731,
            //    TemperatureKelvin = 100,
            //};

            //// Act
            //model = _service.CalculateMappedValues(model);

            //// Assert
            //Assert.Equal("04:41", model.SunRise);
            //Assert.Equal("19:28", model.SunSet);
            //Assert.Equal(-173.1, model.TemperatureCelsius);
            //Assert.Equal(0, model.TemperatureFahrenheit);
            //Assert.Equal(100, model.TemperatureKelvin);

            //criteria.TemperatureUOM = TemperatureUOM.Fahrenheit;
            //model = _service.CalculateMappedValues(model);
            //Assert.Equal(-279.4, model.TemperatureFahrenheit);

            Assert.True(false);
        }
    }
}