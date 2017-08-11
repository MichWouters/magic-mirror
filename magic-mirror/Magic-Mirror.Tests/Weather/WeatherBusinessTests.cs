using System;
using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherBussinessTests
    {
        private readonly IService<WeatherModel> _service;
        private SearchCriteria _criteria;

        public WeatherBussinessTests()
        {
            _criteria = new SearchCriteria
            {
                City = "London"
            };

            _service = new WeatherService();
        }

        [Fact]
        public async Task AutoMapped_Fields_Correct()
        {
            // Arrange
            WeatherModel model = await _service.GetModelAsync(_criteria);

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
            // Arrange
            WeatherModel model = new WeatherModel
            {
                SunRiseMilliseconds = 1502426469,
                SunSetMilliSeconds = 1502479731,
                TemperatureKelvin = 100,
            };

            // Act
            model = _service.CalculateMappedValues(model);

            // Assert
            Assert.Equal("04:41", model.SunRise);
            Assert.Equal("19:28", model.SunSet);
            Assert.Equal(-173.15, model.TemperatureCelsius);
            Assert.Equal(-279, model.TemperatureFahrenheit);
            Assert.Equal(100, model.TemperatureKelvin);

        }
    }
}