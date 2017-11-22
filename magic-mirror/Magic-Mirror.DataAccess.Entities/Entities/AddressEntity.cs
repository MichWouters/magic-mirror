using MagicMirror.DataAccess.Entities.Traffic;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class AddressEntity : Entity
    {
        public Result[] Results { get; set; }
        public string Status { get; set; }
    }
}