using System;

namespace MagicMirror.DataAccess.Entities.User
{
    public class AddressType : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}