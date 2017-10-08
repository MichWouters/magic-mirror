using System;

namespace MagicMirror.DataAccess.Entities.User
{
    public class Address : IEntity
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberSuffix { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
    }
}