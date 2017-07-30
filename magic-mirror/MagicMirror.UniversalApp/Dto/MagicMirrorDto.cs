using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.Dto
{
    public class MagicMirrorDto
    {
        private string _time;

        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private string _compliment;

        public string Compliment
        {
            get { return _compliment; }
            set { _compliment = value; }
        }

        private WeatherModel _weatherModel;

        public WeatherModel WeatherModel
        {
            get { return _weatherModel; }
            set { _weatherModel = value; }
        }

        private TrafficModel _trafficModel;

        public TrafficModel TrafficModel
        {
            get { return _trafficModel; }
            set { _trafficModel = value; }
        }
    }
}