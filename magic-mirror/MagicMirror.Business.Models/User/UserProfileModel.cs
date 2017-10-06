using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.Business.Models.User
{
    public class UserProfileModel: IModel
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid[] FaceIds { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AddressModel> Addresses { get; set; }
    }

    public class AddressModel
    {
        public Guid Id { get; set; }
        public int AddressTypeId { get; set; }
        public int AddressTypeName { get; set; }
        public int AddressId { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHouseNumber { get; set; }
        public string AddressHouseNumberSuffix { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCity { get; set; }
    }
}
