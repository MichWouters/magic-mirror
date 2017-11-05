using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class TrafficDataTests
    {
        private readonly IApiRepo<TrafficEntity> _repo;

        public TrafficDataTests()
        {
            string start = "Generaal Armstrongweg 1 Antwerpen, Belgium";
            string destination = "Earl Bakkenstraat 10, 6422 PJ Heerlen, Netherlands";

            _repo = new TrafficRepo(start, destination);
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
    }
}