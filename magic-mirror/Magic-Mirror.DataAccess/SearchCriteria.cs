namespace MagicMirror.DataAccess
{
    /// <summary>
    /// Generic helper class to pass search parameters from the business to the data-layer.
    /// </summary>
    public class SearchCriteria
    {
        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _start;

        public string Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private string _destination;

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }
    }
}