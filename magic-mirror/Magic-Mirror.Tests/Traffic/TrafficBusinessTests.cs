using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class TrafficBusinessTests
    {
        private readonly IApiRepo<TrafficEntity> _repo;
        private readonly IApiService<TrafficModel> _service;

        public TrafficBusinessTests()
        {
            var criteria = new UserSettings()
            {
                HomeAddress = "Generaal Armstrongweg 1",
                HomeCity = "Antwerpen",
                WorkAddress = "Blarenberglaan 3B - 2800 Mechelen",
            };

            _repo = new TrafficRepo(criteria.HomeAddress, criteria.WorkAddress);
            _service = new TrafficService(criteria);
        }

        [Fact]
        public async Task AutoMapped_Fields_Correct()
        {
            // Arrange
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Act
            TrafficModel model = await _service.GetModelAsync();

            // Assert
            Assert.NotNull(model.DistanceKilometers);
            Assert.NotEqual("0", model.DistanceKilometers);

            Assert.NotNull(model.Minutes);
            Assert.NotEqual(0, model.Minutes);

            Assert.NotNull(model.MinutesText);
            Assert.NotEqual("", model.MinutesText);
            Assert.Equal(model.MinutesText, entity.Routes[0].Legs[0].Duration.Text);
        }
    }
}