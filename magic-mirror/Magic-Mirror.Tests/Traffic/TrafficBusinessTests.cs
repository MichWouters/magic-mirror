using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Services;
using MagicMirror.Business.Services.Traffic;
using MagicMirror.DataAccess;
using MagicMirror.Entities.Traffic;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class TrafficBusinessTests
    {
        private IRepo<TrafficEntity> _repo;
        private IService<TrafficModel> _service;

        public TrafficBusinessTests()
        {
            SearchCriteria criteria = new SearchCriteria
            {
                Start = "Heikant 51 339° Houwaart",
                Destination = "Generaal Armstrongweg 1 Antwerpen"
            };

            _repo = new TrafficRepo(criteria);
            _service = new TrafficService();
        }

        [Fact]
        public async Task Can_Map_Entity_To_Model()
        {
            // Arrange
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Act
            TrafficModel model = _service.MapEntityToModel(entity);

            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public async Task AutoMapped_Fields_Correct()
        {
            // Arrange
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Act
            TrafficModel model = _service.MapEntityToModel(entity);

            // Assert
            Assert.NotNull(model.Distance);
            Assert.NotEqual("", model.Distance);
            Assert.Equal(model.Distance, entity.Routes[0].Legs[0].Distance.Text);

            Assert.NotNull(model.Minutes);
            Assert.NotEqual(0, model.Minutes);
            Assert.Equal(model.Minutes, entity.Routes[0].Legs[0].Duration.Value);

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
