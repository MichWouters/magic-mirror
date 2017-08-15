using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
            Initialize();
            SetRefreshTimers();
        }

        private void Initialize()
        {
            RefreshTime(null, null);
            RefreshWeatherModel(null,null);
            RefreshTrafficModel(null, null);
        }

        private void SetRefreshTimers()
        {
            var timeTimer = new DispatcherTimer();
            timeTimer.Tick += RefreshTime;
            timeTimer.Interval = new TimeSpan(0, 0, 1);
            timeTimer.Start();

            var weatherTimer = new DispatcherTimer();
            weatherTimer.Tick += RefreshWeatherModel;
            weatherTimer.Interval = new TimeSpan(0, 30, 0);
            weatherTimer.Start();

            var trafficTimer = new DispatcherTimer();
            trafficTimer.Tick += RefreshTrafficModel;
            trafficTimer.Interval = new TimeSpan(0, 10, 1);
            trafficTimer.Start();
        }

        private void RefreshTime(object sender, object e)
        {
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        private void RefreshWeatherModel(object sender, object e)
        {
            WeatherModel result = Task.Run(() => _weatherService.GetModelAsync(_searchCriteria)).Result;
            Weather = result;
        }

        private void RefreshTrafficModel(object sender, object e)
        {
            TrafficModel result = Task.Run(() => _trafficService.GetModelAsync(_searchCriteria)).Result;
            Traffic = result;
        }

        #region Properties

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
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public string Compliment => "You look awful today";

        #endregion

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}