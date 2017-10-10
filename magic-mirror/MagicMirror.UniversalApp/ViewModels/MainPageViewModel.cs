using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        // Services from the Business Layer
        private IApiService<WeatherModel> _weatherService;
        private IApiService<RSSModel> _rssService;
        private IApiService<TrafficModel> _trafficService;
        private ISettingsService _settingsService;
        private CommonService _commonService;

        // Timers to refresh individual components
        private DispatcherTimer timeTimer;
        private DispatcherTimer complimentTimer;
        private DispatcherTimer weatherTimer;
        private DispatcherTimer trafficTimer;
        private DispatcherTimer rssTimer;

        public MainPageViewModel()
        {
            SetUpServices();
            SetUpTimers();
            LoadDataOnPageStartup();
            SetRefreshTimers();
        }

        #region Methods
        private void SetUpServices()
        {
            try
            {
                _settingsService = new SettingsService();
                UserSettings userSettings = _settingsService.ReadSettings(localFolder, SETTING_FILE);

                _weatherService = new WeatherService(userSettings);
                _trafficService = new TrafficService(userSettings);
                _commonService = new CommonService();
                _rssService = new RSSService();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to initialize one or more services.", ex.Message);
            }
        }

        private void SetUpTimers()
        {
            try
            {
                timeTimer = new DispatcherTimer();
                complimentTimer = new DispatcherTimer();
                weatherTimer = new DispatcherTimer();
                rssTimer = new DispatcherTimer();
                trafficTimer = new DispatcherTimer();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to initialize one or more timers.", ex.Message);
            }
        }

        // Call data immediately after app launch
        private void LoadDataOnPageStartup()
        {
            GetTime(null, null);
            GetCompliment(null, null);
            RefreshWeatherModel(null, null);
            RefreshTrafficModel(null, null);
            RefreshRSSModel(null, null);
        }

        // Set timers at which data needs to be refreshed
        private void SetRefreshTimers()
        {
            SetUpTimer(timeTimer, new TimeSpan(0, 0, 1), GetTime);
            SetUpTimer(complimentTimer, new TimeSpan(0, 5, 0), GetCompliment);
            SetUpTimer(weatherTimer, new TimeSpan(0, 15, 0), RefreshWeatherModel);
            SetUpTimer(trafficTimer, new TimeSpan(0, 10, 0), RefreshTrafficModel);
            SetUpTimer(rssTimer, new TimeSpan(0, 10, 0), RefreshRSSModel);
        }

        private void SetUpTimer(DispatcherTimer timer, TimeSpan timeSpan, EventHandler<object> method)
        {
            timeTimer.Tick += method;
            timeTimer.Interval = new TimeSpan(0, 0, 10);
            if (!timeTimer.IsEnabled) timeTimer.Start();
        }

        private void GetTime(object sender, object e)
        {
            try { Time = DateTime.Now.ToString("HH:mm"); }
            catch (Exception ex) { DisplayErrorMessage("Cannot set Time", ex.Message); }
        }

        private void GetCompliment(object sender, object e)
        {
            try
            {
                if (_commonService == null) _commonService = new CommonService();

                Compliment = _commonService.GenerateCompliment();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Cannot set Compliment", ex.Message);
            }
        }

        private async void RefreshWeatherModel(object sender, object e)
        {
            try
            {
                WeatherModel weatherModel = await _weatherService.GetModelAsync();
                WeatherInfo = weatherModel;

                if (!weatherTimer.IsEnabled) weatherTimer.Start();
            }
            catch (HttpRequestException)
            {
                // No internet connection. Display dummy data.
                WeatherModel weatherModel = _weatherService.GetOfflineModel(localFolder);
                WeatherInfo = weatherModel;

                // Try to refresh data. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshWeatherModel(null, null);
            }
            catch (Exception)
            {
                // Can't connect to server. Try again after waiting for a few minutes.
                //DisplayErrorMessage("Can't update Weather information", ex.Message);
                if (weatherTimer.IsEnabled) weatherTimer.Stop();

                // Try to refresh data. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshWeatherModel(null, null);
            }
        }

        private async void RefreshRSSModel(object sender, object e)
        {
            try
            {
                RSSModel rssModel = await _rssService.GetModelAsync();
                RSSInfo = rssModel;

                if (!rssTimer.IsEnabled) rssTimer.Start();
            }
            catch (HttpRequestException)
            {
                var rssModel = _rssService.GetOfflineModel(localFolder);
                RSSInfo = rssModel;

                int minutes = 30;
                await Task.Delay((minutes * 60) * 10000);
                RefreshRSSModel(null, null);
            }
            catch (Exception)
            {
                if (rssTimer.IsEnabled) rssTimer.Stop();

                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshRSSModel(null, null);
            }
        }

        private async void RefreshTrafficModel(object sender, object e)
        {
            try
            {
                TrafficModel result = await _trafficService.GetModelAsync();
                TrafficInfo = result;

                if (!trafficTimer.IsEnabled) trafficTimer.Start();
            }
            catch (HttpRequestException)
            {
                TrafficModel trafficModel = _trafficService.GetOfflineModel(localFolder);
                TrafficInfo = trafficModel;

                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshTrafficModel(null, null);
            }
            catch (Exception)
            {
                if (trafficTimer.IsEnabled) trafficTimer.Stop();

                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshTrafficModel(null, null);
            }
        }

        #endregion

        #region Navigation
        public void NavigateToSettings()
        {
            try
            {
                _navigationService.Navigate(typeof(SettingPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Settings", ex.Message);
            }
        }

        public void NavigateToOfflineData()
        {
            try
            {
                _navigationService.Navigate(typeof(OffllineDataPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Offline Data", ex.Message);
            }
        }
        #endregion

        #region Properties

        private WeatherModel _weather;

        public WeatherModel WeatherInfo
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged();
            }
        }

        private RSSModel _rss;

        public RSSModel RSSInfo
        {
            get => _rss;
            set
            {
                _rss = value;
                OnPropertyChanged();
            }
        }

        private TrafficModel _traffic;

        public TrafficModel TrafficInfo
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

        #endregion Properties
    }
}