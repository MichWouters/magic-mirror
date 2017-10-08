using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Entities.User
{
    public class UserEntity: IEntity
    {
        public UserEntity()
        {
            Addresses = new List<UserAddres>();
            Faces = new List<UserFace>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserAddres> Addresses { get; set; }
        public List<UserFace> Faces { get; set; }
        public Guid PersonId { get; set; }
    }
}
