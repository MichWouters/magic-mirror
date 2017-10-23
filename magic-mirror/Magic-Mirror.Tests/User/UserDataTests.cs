using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities.User;
using MagicMirror.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.Tests.User
{
    public class UserDataTests
    {
        private readonly SqliteContext _context;
        private readonly UserRepo _repo;

        public UserDataTests()
        {
            _context = new SqliteContext("tests.db");
            _repo = new UserRepo(_context);
        }

        [Fact]
        public async Task Can_Change_And_Retrieve_User()
        {
            var faceId = Guid.NewGuid();
            var entity = await _repo.AddUserAsync(new UserEntity
            {
                FirstName = "Jon",
                LastName = "Stark",
                Addresses = new List<UserAddress>
                {
                    new UserAddress
                    {
                        AddressType = new AddressType
                        {
                            Name = "Woonplaats"
                        },
                        Address = new Address
                        {
                            Street = "Old Course",
                            HouseNumber = "7",
                            Postcode = "BT30",
                            City = "Winterfell"
                        }
                    }
                }
            });

            var result = await _repo.GetUserByPersonId(faceId);

            Assert.NotNull(result);
            Assert.Equal("Jon", result.FirstName);

            _context.DeleteDatabase();
        }

        [Fact]
        public async Task Can_Update_User()
        {
            var faceId = Guid.NewGuid();
            var entity = await _repo.AddUserAsync(new UserEntity
            {
                FirstName = "Jon",
                LastName = "Stark",
                Addresses = new List<UserAddress>
                {
                    new UserAddress
                    {
                        AddressType = new AddressType
                        {
                            Name = "Woonplaats"
                        },
                        Address = new Address
                        {
                            Street = "Old Course",
                            HouseNumber = "7",
                            Postcode = "BT30",
                            City = "Winterfell"
                        }
                    }
                }
            });

            var result = await _repo.GetUserByPersonId(faceId);

            result.Addresses.First().Address.HouseNumber = "100";
            result.LastName = "Snow";

            var changedEntity = await _repo.UpdateUserAsync(result);

            var result2 = await _repo.GetUserByPersonId(faceId);

            Assert.NotNull(result2);
            Assert.Equal(result.LastName, result2.LastName);
            Assert.Equal(result.Addresses.First().Address.HouseNumber, result2.Addresses.First().Address.HouseNumber);

            _context.DeleteDatabase();
        }

        [Fact]
        public async Task Can_Delete_User()
        {
            var faceId = Guid.NewGuid();
            var entity = await _repo.AddUserAsync(new UserEntity
            {
                FirstName = "Jon",
                LastName = "Stark",
                Addresses = new List<UserAddress>
                {
                    new UserAddress
                    {
                        AddressType = new AddressType
                        {
                            Name = "Woonplaats"
                        },
                        Address = new Address
                        {
                            Street = "Old Course",
                            HouseNumber = "7",
                            Postcode = "BT30",
                            City = "Winterfell"
                        }
                    }
                }
            });

            var result = await _repo.GetUserByPersonId(faceId);

            await _repo.DeleteUserAsync(result);

            var result2 = await _repo.GetUserByPersonId(faceId);

            _context.DeleteDatabase();
        }
    }
}