using System;
using System.Collections.Generic;

namespace MagicMirror.Business.Models.User
{
    public class UserProfileModel : IModel
    {
        public UserProfileModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid[] FaceIds { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserAddressModel> Addresses { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }

    public class UserAddressModel
    {
        public Guid Id { get; set; }
        public Guid AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public Guid AddressId { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHouseNumber { get; set; }
        public string AddressHouseNumberSuffix { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCity { get; set; }
    }

    public static class UserProfileModelExtensions
    {
        private static Random _random = new Random();

        public static UserProfileModel RandomData(this UserProfileModel model)
        {
            model.FirstName = "User";
            model.LastName = _random.Next(100, 999).ToString();
            model.Addresses = new List<UserAddressModel>
            {
                new UserAddressModel
                {
                    Id = Guid.NewGuid(),
                    AddressTypeName = "Ordina Hasselt",
                    AddressStreet = "Gouverneur Roppesingel",
                    AddressHouseNumber = "25",
                    AddressPostCode = "2500",
                    AddressCity = "Hasselt",
                },
                new UserAddressModel
                {
                    Id = Guid.NewGuid(),
                    AddressTypeName = "Ordina HQ",
                    AddressStreet = "Blarenberglaan",
                    AddressHouseNumber = "3B",
                    AddressPostCode = "2800",
                    AddressCity = "Mechelen",
                }
            };

            return model;
        }
    }
}