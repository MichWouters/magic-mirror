using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Entities.Weather;
using MagicMirror.DataAccess.Repos;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Weather
{
    public class WeatherDataAccessTests
    {
        private readonly IApiRepo<WeatherEntity> _repo;

        public WeatherDataAccessTests()
        {
            _repo = new WeatherRepo("London");
        }

        // If this test fails, we know why other test fail
        [Fact]
        public async Task Can_Connect_To_Api()
        {
            // Act
            var result = await _repo.GetHttpResponseFromApiAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal("200", result.StatusCode.ToString());
        }

        [Fact]
        public async Task Result_Not_Null()
        {
            // Act
            var result = await _repo.GetEntityAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Convert_Json_To_Entity_Success()
        {
            // Act
            var result = await _repo.GetEntityAsync();

            // Assert
            Assert.Equal(result.Name.ToLower().Trim(), "london");
            Assert.IsType<WeatherEntity>(result);
        }
    }
}