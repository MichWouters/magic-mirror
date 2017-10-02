using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Entities.User
{
    public class UserEntity: IEntity
    {
        public int Id { get; set; }
        public Guid FaceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserAddres> Addresses { get; set; }
    }
}
