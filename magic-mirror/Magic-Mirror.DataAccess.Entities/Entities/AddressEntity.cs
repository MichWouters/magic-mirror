using MagicMirror.DataAccess.Entities.Traffic;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class AddressEntity : IEntity
    {
        public Result[] results { get; set; }
        public string status { get; set; }
    }
}