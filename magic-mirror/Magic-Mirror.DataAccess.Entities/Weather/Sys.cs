namespace MagicMirror.DataAccess.Entities.Weather
{
    public class Sys
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public double Message { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }
}
