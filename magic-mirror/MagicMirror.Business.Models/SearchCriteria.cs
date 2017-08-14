namespace MagicMirror.Business.Models
{
    /// <summary>
    /// Generic helper class to pass search parameters from the business to the data-layer.
    /// </summary>
    public class SearchCriteria
    {
        public string UserName { get; set; }

        public string City { get; set; }

        public string Start { get; set; }

        public string Destination { get; set; }

        public override string ToString()
        {
            return $"Name: {this.UserName}\n" +
                   $"City: {this.City}";
        }
    }
}