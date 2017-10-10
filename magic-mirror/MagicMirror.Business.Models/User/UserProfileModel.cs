using System;
using System.Collections.Generic;
using System.Linq;

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
        private static Tuple<string, string>[] _maleNames = new Tuple<string, string>[]
        {
            new Tuple<string, string>("Eddard", "Stark"),
            new Tuple<string, string>("Robert", "Baratheon"),
            new Tuple<string, string>("Jaime", "Lannister"),
            new Tuple<string, string>("Jorah", "Mormont"),
            new Tuple<string, string>("Petyr", "Baelish"),
            new Tuple<string, string>("Viserys", "Targaryen"),
            new Tuple<string, string>("Jon", "Snow"),
            new Tuple<string, string>("Robb", "Stark"),
            new Tuple<string, string>("Theon", "Greyjoy"),
            new Tuple<string, string>("Bran", "Stark"),
            new Tuple<string, string>("Joffrey", "Baratheon"),
            new Tuple<string, string>("Sandor", "Clegane"),
            new Tuple<string, string>("Tyrion", "Lannister"),
            new Tuple<string, string>("Khal", "Drogo"),
            new Tuple<string, string>("Tywin", "Lannister"),
            new Tuple<string, string>("Davos", "Seaworth"),
            new Tuple<string, string>("Samwell", "Tarly"),
            new Tuple<string, string>("Stannis", "Baratheon"),
            new Tuple<string, string>("Jeor", "Mormont"),
            new Tuple<string, string>("Tormund", "Giantsbane"),
            new Tuple<string, string>("Ramsay", "Bolton"),
            new Tuple<string, string>("Daario", "Naharis"),
            new Tuple<string, string>("Jaqen", "H'ghar"),
            new Tuple<string, string>("Tommen", "Baratheon"),
            new Tuple<string, string>("Roose", "Bolton"),
            new Tuple<string, string>("The High", "Sparrow"),
        };
        private static Tuple<string, string>[] _femaleNames = new Tuple<string, string>[]
        {
            new Tuple<string, string>("Catelyn", "Stark"),
            new Tuple<string, string>("Cersei", "Lannister"),
            new Tuple<string, string>("Daenerys", "Targaryen"),
            new Tuple<string, string>("Sansa", "Stark"),
            new Tuple<string, string>("Arya", "Stark"),
            new Tuple<string, string>("Margaery", "Tyrell"),
            new Tuple<string, string>("Talisa", "Maegyr"),
            new Tuple<string, string>("Brienne", "of Tarth"),
            new Tuple<string, string>("Ellaria", "Sand")
        };
        private static Random _random = new Random();

        public static UserProfileModel RandomData(this UserProfileModel model, string gender)
        {
            Tuple<string, string> name;
            int ix;

            switch (gender.ToUpper())
            {
                case "FEMALE":
                    ix = _random.Next(0, _femaleNames.Length);
                    name = _femaleNames[ix];
                    break;
                default:
                    ix = _random.Next(0, _maleNames.Length);
                    name = _maleNames[ix];
                    break;
            }
            
            model.FirstName = name.Item1;
            model.LastName = name.Item2;
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