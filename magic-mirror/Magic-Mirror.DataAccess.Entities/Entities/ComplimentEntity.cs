namespace MagicMirror.DataAccess.Entities.Entities
{
    public class ComplimentEntity : IIdentifiableEntity
    {
        private int _entityId;

        public int EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}