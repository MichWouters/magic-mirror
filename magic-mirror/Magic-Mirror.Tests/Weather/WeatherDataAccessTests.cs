using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.Weather;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherDataAccessTests
    {
        private IRepo<WeatherEntity> _repo;
        private SearchCriteria criteria;

        public WeatherDataAccessTests()
        {
            criteria = new SearchCriteria { City = "London" };
            _repo = new WeatherRepo(criteria);
        }

        [Fact]
        public async Task RetrieveJsonData()
        {
            // Act
            var result = await _repo.GetJsonAsync();

            // Assert
            Assert.NotEqual("", result);
            Assert.NotEqual(0, result.Length);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ConvertJsonToEntity()
        {
            // Act
            var result = await _repo.GetEntityAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name.ToLower().Trim(), criteria.City.ToLower().Trim());
            Assert.IsType<WeatherEntity>(result);
        }

        [Fact]
        public async Task CanConnectToApi()
        {
            // Act
            var result = await _repo.GetJsonResponseAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void EmptyInputRaisesException()
        {
            // Arrange
            var exception = Record.Exception(() => new WeatherRepo(null));
            var exception2 = Record.Exception(() => new WeatherRepo(new SearchCriteria()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType(typeof(ArgumentNullException), exception);

            Assert.NotNull(exception2);
            Assert.IsType(typeof(ArgumentException), exception2);
        }
    }
}