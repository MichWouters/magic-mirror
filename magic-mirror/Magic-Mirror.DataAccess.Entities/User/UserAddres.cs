namespace MagicMirror.DataAccess.Entities.User
{
    public class UserAddres : IEntity
    {
        public int Id { get; set; }
        public AddressType AddressType { get; set; }
        public Address Address { get; set; }
    }
}