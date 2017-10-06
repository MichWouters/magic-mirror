using System;

namespace MagicMirror.DataAccess.Entities.User
{
    public class UserAddres : IEntity
    {
        public Guid Id { get; set; }
        public AddressType AddressType { get; set; }
        public Address Address { get; set; }
    }
}