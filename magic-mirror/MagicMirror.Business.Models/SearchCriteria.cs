namespace MagicMirror.Business.Models
{
    /// <summary>
    /// Generic helper class to pass search parameters from the business to the data-layer.
    /// </summary>
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Precision = 2;
            TemperatureUOM = TemperatureUOM.Celsius;
            DistanceUOM = DistanceUOM.Metric;
        }

        public string UserName { get; set; }

        public string HomeCity { get; set; }

        public string HomeAddress { get; set; }

        public string WorkAddress { get; set; }

        public int Precision { get; set; }

        public TemperatureUOM TemperatureUOM { get; set; }

        public DistanceUOM DistanceUOM { get; set; }

        public override string ToString()
        {
            return $"Name: {this.UserName}\n" +
                   $"HomeCity: {this.HomeCity}";
        }
    }

    


}