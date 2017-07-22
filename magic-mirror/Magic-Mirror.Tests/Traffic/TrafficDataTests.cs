using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Traffic;
using MagicMirror.Entities.Traffic;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.UnitTests.Traffic
{
    public class TrafficDataTests
    {
        private IRepo<TrafficEntity> _repo;
        private SearchCriteria _criteria;

        public TrafficDataTests()
        {
            _criteria = new SearchCriteria
            {
                Start = "Heikant 51 Houwaart Belgium",
                Destination = "Earl Bakkenstraat 10, 6422 PJ Heerlen, Netherlands"
            };

            _repo = new TrafficRepo(_criteria);
        }

        [Fact]
        [DisplayName("Foo")]
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
        public async Task Json_Can_Convert_To_Entity()
        {
            // Act
            TrafficEntity result = await _repo.GetEntityAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual("NOT_FOUND", result.Status);
            Assert.NotEqual("ZERO_RESULTS", result.Status);
            Assert.Equal("OK", result.Status);
            Assert.NotNull(result.Routes[0].Legs[0].Duration);
            Assert.NotEqual(0, result.Routes[0].Legs[0].Duration.Value);
        }

        [Fact]
        public void Empty_Input_Throws_Exception()
        {
            SearchCriteria criteria = null; ;
            var exception = AssertEmptyInput(criteria, typeof(NullReferenceException));

            criteria = new SearchCriteria();
            exception = AssertEmptyInput(criteria, typeof(ArgumentException));

            criteria.Start = "";
            criteria.Destination = "London";
            exception = AssertEmptyInput(criteria, typeof(ArgumentException));

            criteria.Start = "London";
            criteria.Destination = "";
            exception = AssertEmptyInput(criteria, typeof(ArgumentException));
        }

        private Exception AssertEmptyInput(SearchCriteria criteria, Type type)
        {
            Exception exception = Record.Exception(() => new TrafficRepo(criteria));
            Assert.NotNull(exception);
            Assert.IsType(type, exception);
            return exception;
        }
    }
}
