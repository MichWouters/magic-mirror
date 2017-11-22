using MagicMirror.DataAccess.Entities.Weather;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class WeatherEntity : Entity
    {
        public Coord Coord { get; set; }
        public Weather.Weather[] Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public int Visibility { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public int Dt { get; set; }
        public Sys Sys { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }
    }
}