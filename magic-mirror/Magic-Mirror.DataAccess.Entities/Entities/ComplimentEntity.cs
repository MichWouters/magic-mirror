namespace MagicMirror.DataAccess.Entities.Entities
{
    public class ComplimentEntity : IIdentifiableEntity
    {
        public int EntityId { get; set; }

        public string Value { get; set; }
    }
}