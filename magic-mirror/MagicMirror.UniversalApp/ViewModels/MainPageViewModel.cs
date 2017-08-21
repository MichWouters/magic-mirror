using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;
        private readonly CommonService _commonService;
        private readonly SearchCriteria _searchCriteria;

        public MainPageViewModel()
        {
            //_searchCriteria = new SearchCriteria
            //{
            //    HomeCity = "Houwaart",
            //    WorkAddress = "Generaal ArmstrongWeg 1, Antwerpen",
            //    HomeAddress = "Heikant 51 Houwaart",
            //    UserName = "Michiel"
            //};

            var appReference = Application.Current as App;
            _searchCriteria = appReference.Criteria;

            _commonService = new CommonService();
            _weatherService = new WeatherService(_searchCriteria);
            _trafficService = new TrafficService(_searchCriteria);

            Initialize();
            SetRefreshTimers();
        }

        private void Initialize()
        {
            RefreshTime(null, null);
            RefreshCompliment(null, null);
            RefreshWeatherModel(null, null);
            RefreshTrafficModel(null, null);
        }

        private void SetRefreshTimers()
        {
            // Todo: Set active hours
            bool activeHours = true;

            var timeTimer = new DispatcherTimer();
            timeTimer.Tick += RefreshTime;
            timeTimer.Interval = new TimeSpan(0, 1, 0);
            timeTimer.Start();

            var complimentTimer = new DispatcherTimer();
            complimentTimer.Tick += RefreshCompliment;
            complimentTimer.Interval = new TimeSpan(0, 10,0);
            complimentTimer.Start();

            if (activeHours)
            {
                var weatherTimer = new DispatcherTimer();
                weatherTimer.Tick += RefreshWeatherModel;
                weatherTimer.Interval = new TimeSpan(0, 30, 0);
                weatherTimer.Start();

                var trafficTimer = new DispatcherTimer();
                trafficTimer.Tick += RefreshTrafficModel;
                trafficTimer.Interval = new TimeSpan(0, 10, 1);
                trafficTimer.Start();
            }
        }

        private void RefreshTime(object sender, object e)
        {
            Time = DateTime.Now.ToString("HH:mm");
        }

        private void RefreshCompliment(object sender, object e)
        {
            Compliment = _commonService.GenerateCompliment();
        }

        private void RefreshWeatherModel(object sender, object e)
        {
            WeatherModel result = Task.Run(() => _weatherService.GetModelAsync()).Result;
            result.Icon = ConvertWeatherIcon(result.Icon);
            Weather = result;
        }

        private void RefreshTrafficModel(object sender, object e)
        {
            TrafficModel result = Task.Run(() => _trafficService.GetModelAsync()).Result;
            Traffic = result;
        }

        private string ConvertWeatherIcon(string icon)
        {
            string theme = "Dark";
            string prefix = "ms-appx:///Assets/Weather";
            string res;

            switch (icon)
            {
                case "01d":
                    res = "01d.png";
                    break;
                case "01n":
                    res = "01n.png";
                    break;
                case "02d":
                    res = "02d.png";
                    break;
                case "02n":
                    res = "02n.png";
                    break;
                case "03d":
                case "03n":
                case "04d":
                case "04n":
                    res = "03or4.png";
                    break;
                case "09n":
                case "09d":
                    res = "09.png";
                    break;
                case "10d":
                case "10n":
                    res = "09.png";
                    break;
                case "11d":
                    res = "11d.png";
                    break;
                case "11n":
                    res = "11n.png";
                    break;
                case "13d":
                case "13n":
                    res = "13.png";
                    break;
                case "50n":
                case "50d":
                default:
                    res = "50.png";
                    break;
            }
            return $"{prefix}/{theme}/{res}";
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

        private string _compliment;

        public string Compliment
        {
            get => _compliment;
            set
            {
                _compliment = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}