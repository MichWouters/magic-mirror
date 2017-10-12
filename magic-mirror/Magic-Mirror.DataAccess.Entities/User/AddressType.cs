using System;

namespace MagicMirror.DataAccess.Entities.User
{
    public class AddressType : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static AddressType Home = new AddressType { Id = new Guid("D02F3D31-778D-45D0-A96F-8719C8521C3F"), Name = "Home" };
        public static AddressType Work = new AddressType { Id = new Guid("58605A64-D640-4709-98AD-5E2F4E2E58C1"), Name = "Work" };
    }
}