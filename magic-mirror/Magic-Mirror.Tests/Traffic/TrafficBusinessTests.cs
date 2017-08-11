using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Repos;
using MagicMirror.Entities.Traffic;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class TrafficBusinessTests
    {
        private readonly IRepo<TrafficEntity> _repo;
        private readonly IService<TrafficModel> _service;
        private readonly SearchCriteria _criteria;

        public TrafficBusinessTests()
        {
            _criteria = new SearchCriteria
            {
                Start = "Heikant 51 339° Houwaart",
                Destination = "Generaal Armstrongweg 1 Antwerpen"
            };

            _repo = new TrafficRepo(_criteria);
            _service = new TrafficService();
        }

        [Fact]
        public async Task AutoMapped_Fields_Correct()
        {
            // Arrange
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Act
            TrafficModel model = await _service.GetModelAsync(_criteria);

            // Assert
            Assert.NotNull(model.Distance);
            Assert.NotEqual("", model.Distance);
            Assert.Equal(model.Distance, entity.Routes[0].Legs[0].Distance.Text);

            Assert.NotNull(model.Minutes);
            Assert.NotEqual(0, model.Minutes);
            Assert.Equal(model.Minutes, entity.Routes[0].Legs[0].Duration.Value / 60);

            Assert.NotNull(model.MinutesText);
            Assert.NotEqual("", model.MinutesText);
            Assert.Equal(model.MinutesText, entity.Routes[0].Legs[0].Duration.Text);
        }

        [Fact]
        public void Calculated_Fields_Correct()
        {
            // Arrange
            TrafficModel model = new TrafficModel
            {
                Minutes = 5400,
                NumberOfIncidents = 3,
            };

            // Act
            model = _service.CalculateMappedValues(model);

            // Assert
            Assert.Equal(90, model.Minutes);
            Assert.Equal(TrafficDensity.Heavy, model.TrafficDensity);
        }
    }
}