using MagicMirror.DataAccess.Entities.Entities;
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