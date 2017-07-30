namespace MagicMirror.DataAccess
{
    /// <summary>
    /// Generic helper class to pass search parameters from the business to the data-layer.
    /// </summary>
    public class SearchCriteria
    {
        public string City { get; set; }

        public string Start { get; set; }

        public string Destination { get; set; }
    }
}