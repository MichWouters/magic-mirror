using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.Traffic
{
    public class AddressDataTests
    {
        private readonly IApiRepo<AddressEntity> _repo;

        public AddressDataTests()
        {
            string latitude = "50.93595";
            string longitude = "4.8529";

            _repo = new AddressRepo(latitude, longitude);
        }
        
        [Fact]
        public async Task Address_Test()
        {
            var result = await _repo.GetEntityAsync();

            Assert.True(false);
        }
    }
}
