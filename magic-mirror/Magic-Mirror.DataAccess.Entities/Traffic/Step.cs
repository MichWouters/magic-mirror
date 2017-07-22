namespace MagicMirror.Entities.Traffic
{
    public class Step
    {
        public Distance2 Distance { get; set; }
        public Duration2 Duration { get; set; }
        public EndLocation2 EndLocation { get; set; }
        public string HtmlInstructions { get; set; }
        public Polyline Polyline { get; set; }
        public StartLocation2 StartLocation { get; set; }
        public string TravelMode { get; set; }
        public string Maneuver { get; set; }
    }
}
