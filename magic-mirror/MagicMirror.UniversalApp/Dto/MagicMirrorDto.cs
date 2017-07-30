using MagicMirror.Business.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagicMirror.UniversalApp.Dto
{
    public class MagicMirrorDto : INotifyPropertyChanged
    {
        private int _foo;

        public int Foo
        {
            get { return _foo; }
            set { _foo = value; }
        }


        private WeatherModel _weatherModel;

        public WeatherModel WeatherModel
        {
            get { return _weatherModel; }
            set
            {
                _weatherModel = value;
                OnPropertyChanged(nameof(WeatherModel));
            }
        }

        private TrafficModel _trafficModel;


        public TrafficModel TrafficModel
        {
            get { return _trafficModel; }
            set
            {
                _trafficModel = value;
                OnPropertyChanged(nameof(TrafficModel));
            }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private string _compliment;

        public string Compliment
        {
            get { return _compliment; }
            set
            {
                _compliment = value;
                OnPropertyChanged(nameof(Compliment));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}