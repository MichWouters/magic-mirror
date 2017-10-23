using System;

namespace MagicMirror.DataAccess.Entities.User
{
    public class UserAddress : IEntity
    {
        public Guid Id { get; set; }
        public AddressType AddressType { get; set; }
        public Address Address { get; set; }
    }
}