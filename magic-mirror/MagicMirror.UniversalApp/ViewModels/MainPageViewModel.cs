using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;
        private readonly SearchCriteria _searchCriteria;

        public MainPageViewModel()
        {
            _searchCriteria = new SearchCriteria
            {
                City = "Houwaart",
                Destination = "Generaal ArmstrongWeg 1, Antwerpen",
                Start = "Heikant 51 Houwaart",
                UserName = "Michiel"
            };
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();
            SetRefreshTimers();
        }

        private void SetRefreshTimers()
        {
            var autoEvent = new AutoResetEvent(false);

            Timer timeTimer = new Timer(RefreshTime, autoEvent, 1000, 1000);
            //Timer weatherTimer = new Timer(RefreshWeatherModel, autoEvent, 1000, 10000);
            //Timer trafficTimer = new Timer(RefreshTrafficModel, autoEvent, 1000, 10000);
        }

        private void RefreshWeatherModel(object state)
        {
            Task.Run(() =>
            {
                WeatherModel result = Task.Run(() => _weatherService.GetModelAsync(_searchCriteria)).Result;
                Weather = result;
            });
        }

        private void RefreshTrafficModel(object state)
        {
            TrafficModel result = Task.Run(() => _trafficService.GetModelAsync(_searchCriteria)).Result;
            Traffic = result;
        }

        private void RefreshTime(object state)
        {
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        private WeatherModel _weather;

        public WeatherModel Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged();
            }
        }

        private TrafficModel _traffic;

        public TrafficModel Traffic
        {
            get => _traffic;
            set
            {
                _traffic = value;
                OnPropertyChanged();
            }
        }

        public DateModel Date => new DateModel();

        private string _time;

        public string Time
        {
            get => DateTime.Now.ToString("HH:mm:ss");
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public string Compliment => "You look awful today";

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}