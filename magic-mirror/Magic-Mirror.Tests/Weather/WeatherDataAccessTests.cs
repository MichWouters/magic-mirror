using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.Weather;
using MagicMirror.DataAccess.Repos;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherDataAccessTests
    {
        private readonly IRepo<WeatherEntity> _repo;

        public WeatherDataAccessTests()
        {
            _repo = new WeatherRepo("London");
        }

        [Fact]
        public async Task Can_Retrieve_Json_Data()
        {
            // Act
            var result = await _repo.GetJsonAsync();

            // Assert
            Assert.NotEqual("", result);
            Assert.NotEqual(0, result.Length);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Convert_Json_To_Entity_Success()
        {
            // Act
            var result = await _repo.GetEntityAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name.ToLower().Trim(), "london");
            Assert.IsType<WeatherEntity>(result);
        }

        [Fact]
        public async Task Connect_To_Api_Success()
        {
            // Act
            var result = await _repo.GetJsonResponseAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}