using System;
using System.Collections.Generic;

namespace MagicMirror.Business.Models.User
{
    public class UserProfileModel : IModel
    {
        public UserProfileModel()
        {
            Id = Guid.NewGuid();
            FaceIds = new List<Guid>();
        }

        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public List<Guid> FaceIds { get; set; }
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

        private static UserAddressModel[] _addresses = new UserAddressModel[]
{
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address1", AddressStreet = "Coolshof", AddressHouseNumber = "18", AddressPostCode = "8497", AddressCity = "Lessen" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address2", AddressStreet = "Christiaensring", AddressHouseNumber = "46", AddressPostCode = "0105", AddressCity = "Florenville" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address3", AddressStreet = "Leclercqpad", AddressHouseNumber = "68", AddressPostCode = "3343", AddressCity = "Geraardsbergen" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address4", AddressStreet = "Verbruggenpad", AddressHouseNumber = "32", AddressPostCode = "9053", AddressCity = "Antoing" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address5", AddressStreet = "Vermeerschlaan", AddressHouseNumber = "86", AddressPostCode = "3046", AddressCity = "Ronse" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address6", AddressStreet = "Timmermanshof", AddressHouseNumber = "84", AddressPostCode = "3938", AddressCity = "Oudenaarde" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address7", AddressStreet = "Jacquetsingel", AddressHouseNumber = "60", AddressPostCode = "2178", AddressCity = "Le Rœulx" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address8", AddressStreet = "Lauwerssingel", AddressHouseNumber = "47", AddressPostCode = "0833", AddressCity = "Spa" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address9", AddressStreet = "Vermeerschbaan", AddressHouseNumber = "14", AddressPostCode = "2941", AddressCity = "Chimay" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address10", AddressStreet = "Thirydreef", AddressHouseNumber = "40", AddressPostCode = "0828", AddressCity = "Damme" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address11", AddressStreet = "Bahboulevard", AddressHouseNumber = "24", AddressPostCode = "4462", AddressCity = "Neufchâteau" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address12", AddressStreet = "Delfosseweg", AddressHouseNumber = "52", AddressPostCode = "1165", AddressCity = "Geraardsbergen" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address13", AddressStreet = "Pietershof", AddressHouseNumber = "73", AddressPostCode = "1351", AddressCity = "Oostende" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address14", AddressStreet = "Piettebaan", AddressHouseNumber = "24", AddressPostCode = "9411", AddressCity = "Geraardsbergen" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address15", AddressStreet = "De Grootelaan", AddressHouseNumber = "17", AddressPostCode = "8320", AddressCity = "Nieuwpoort" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address16", AddressStreet = "Leclercqboulevard", AddressHouseNumber = "71", AddressPostCode = "9568", AddressCity = "Antwerpen" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address17", AddressStreet = "De Greefboulevard", AddressHouseNumber = "63", AddressPostCode = "0655", AddressCity = "Péruwelz" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address18", AddressStreet = "Yildirimsteeg", AddressHouseNumber = "54", AddressPostCode = "5457", AddressCity = "Herk-de-Stad" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address19", AddressStreet = "Baertlaan", AddressHouseNumber = "52", AddressPostCode = "9601", AddressCity = "Chimay" },
            new UserAddressModel { Id = Guid.NewGuid(), AddressTypeName = "Address20", AddressStreet = "Van Dyckweg", AddressHouseNumber = "98", AddressPostCode = "1311", AddressCity = "Brussel" }
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
                _addresses[_random.Next(0, _addresses.Length)],
                _addresses[_random.Next(0, _addresses.Length)]
            };

            model.Addresses[0].AddressTypeId = new Guid("D02F3D31-778D-45D0-A96F-8719C8521C3F");
            model.Addresses[0].AddressTypeName = "Home";
            model.Addresses[1].AddressTypeId = new Guid("58605A64-D640-4709-98AD-5E2F4E2E58C1");
            model.Addresses[1].AddressTypeName = "Work";

            return model;
        }
    }
}