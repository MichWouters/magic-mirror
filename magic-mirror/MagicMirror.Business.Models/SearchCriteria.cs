namespace MagicMirror.Business.Models
{
    /// <summary>
    /// Generic helper class to pass search parameters from the business to the data-layer.
    /// </summary>
    public class SearchCriteria
    {
        public string UserName { get; set; }

        public string HomeCity { get; set; }

        public string HomeAddress { get; set; }

        public string WorkAddress { get; set; }

        public override string ToString()
        {
            return $"Name: {this.UserName}\n" +
                   $"HomeCity: {this.HomeCity}";
        }
    }
}