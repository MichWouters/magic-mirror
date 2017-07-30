using MagicMirror.DataAccess;
using MagicMirror.Entities.Traffic;
using System;
using System.Net;
using System.Threading.Tasks;
using MagicMirror.DataAccess.Repos;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class TrafficDataTests
    {
        private readonly IRepo<TrafficEntity> _repo;

        public TrafficDataTests()
        {
            var criteria = new SearchCriteria
            {
                Start = "Heikant 51 Houwaart Belgium",
                Destination = "Earl Bakkenstraat 10, 6422 PJ Heerlen, Netherlands"
            };

            _repo = new TrafficRepo(criteria);
        }

        [Fact]
        public async Task Can_Access_Api()
        {
            var result = await _repo.GetJsonResponseAsync();

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Can_Retrieve_Json()
        {
            // Act
            string result = await _repo.GetJsonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual("", result);
        }

        [Fact]
        public async Task Can_Convert_Json_To_Entity()
        {
            // Act
            TrafficEntity result = await _repo.GetEntityAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual("NOT_FOUND", result.Status);
            Assert.NotEqual("ZERO_RESULTS", result.Status);
            Assert.Equal("OK", result.Status);
            Assert.NotEqual(0, result.Routes.Count);
            Assert.NotNull(result.Routes[0].Legs[0].Duration);
            Assert.NotEqual(0, result.Routes[0].Legs[0].Duration.Value);
        }

        [Fact]
        public void Empty_Input_Throws_Exception()
        {
            AssertEmptyInput(null, typeof(NullReferenceException));

            var criteria = new SearchCriteria();
            AssertEmptyInput(criteria, typeof(ArgumentException));

            criteria.Start = "";
            criteria.Destination = "London";
            AssertEmptyInput(criteria, typeof(ArgumentException));

            criteria.Start = "London";
            criteria.Destination = "";
            AssertEmptyInput(criteria, typeof(ArgumentException));
        }

        private void AssertEmptyInput(SearchCriteria criteria, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Exception exception = Record.Exception(() => new TrafficRepo(criteria));
            Assert.NotNull(exception);
            Assert.IsType(type, exception);
        }
    }
}