using Acme.Generic;
using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Common;
using MagicMirror.UniversalApp.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region privates

        /* Services from the Business Layer */
        private IWeatherService _weatherService;
        private ITrafficService _trafficService;
        private IRSSService _rssService;
        private ISettingsService _settingsService;
        private ICommonService _commonService;

        /* Timers to refresh individual components */
        private DispatcherTimer timeTimer;
        private DispatcherTimer dateTimer;
        private DispatcherTimer complimentTimer;
        private DispatcherTimer weatherTimer;
        private DispatcherTimer trafficTimer;
        private DispatcherTimer rssTimer;

        /* Properties */
        private double temperature;
        private string weatherimage;
        private string weatherType;
        private string sunrise;
        private string sunset;
        private string compliment;
        private string travelDuration;
        private string location;
        private DateTime date;
        private string time;
        private RSSModel _rss;

        #endregion privates

        public MainPageViewModel(IWeatherService weatherService, ITrafficService trafficService, IRSSService rSSService, ISettingsService settingsService, ICommonService commonService)
        {
            // Constructor injection
            _weatherService = weatherService;
            _trafficService = trafficService;
            _rssService = rSSService;
            _settingsService = settingsService;
            _commonService = commonService;

            App.UserSettings = _settingsService.LoadSettings();
            InitializeTimers();
            LoadDataOnPageStartup();
            SetRefreshTimers();

            GoToSettings = new CustomCommand(NavigateToSettings, CanNavigateToSettings);
        }

        public ICommand GoToSettings { get; set; }

        private void NavigateToSettings(object obj)
        {
            base.NavigateToSettings();
        }

        private bool CanNavigateToSettings(object obj)
        {
            return true;
        }

        #region Methods

        private void InitializeTimers()
        {
            try
            {
                timeTimer = new DispatcherTimer();
                dateTimer = new DispatcherTimer();
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

        private void LoadDataOnPageStartup()
        {
            GetTime(null, null);
            GetTime(null, null);
            GetCompliment(null, null);
            RefreshWeatherModel(null, null);
            RefreshTrafficModel(null, null);
            RefreshRSSModel(null, null);
        }

        private void SetRefreshTimers()
        {
            // Set timers at which data needs to be refreshed
            SetUpTimer(timeTimer, new TimeSpan(0, 0, 1), GetTime);
            SetUpTimer(dateTimer, new TimeSpan(1, 0, 0), GetDate);
            SetUpTimer(complimentTimer, new TimeSpan(0, 5, 0), GetCompliment);
            SetUpTimer(weatherTimer, new TimeSpan(0, 15, 0), RefreshWeatherModel);
            SetUpTimer(trafficTimer, new TimeSpan(0, 10, 0), RefreshTrafficModel);
            SetUpTimer(rssTimer, new TimeSpan(0, 10, 0), RefreshRSSModel);
        }

        private void SetUpTimer(DispatcherTimer timer, TimeSpan timeSpan, EventHandler<object> method)
        {
            timer.Tick += method;
            timer.Interval = timeSpan;
            if (!timer.IsEnabled) timer.Start();
        }

        private void GetTime(object sender, object e)
        {
            try { Time = DateHelper.CurrentTime; }
            catch (Exception ex) { DisplayErrorMessage("Cannot set Time", ex.Message); }
        }

        private void GetDate(object sender, object e)
        {
            try { Time = DateHelper.CurrentTimeFull; }
            catch (Exception ex) { DisplayErrorMessage("Cannot set Date", ex.Message); }
        }

        private void GetCompliment(object sender, object e)
        {
            try
            {
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
                var settings = App.UserSettings;
                WeatherModel weatherModel = await _weatherService.GetModelAsync(settings.HomeCity, settings.Precision, settings.TemperatureUOM);
                FillWeatherInformation(weatherModel);

                if (!weatherTimer.IsEnabled) weatherTimer.Start();
            }
            catch (HttpRequestException ex)
            {
                // No internet connection. Display dummy data.
                DisplayErrorMessage("No internet connection. Displaying dummy data", ex.Message);
                WeatherModel weatherModel = _weatherService.GetOfflineModel(localFolder);
                FillWeatherInformation(weatherModel);

                // Try to refresh data. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshWeatherModel(null, null);
            }
            catch (Exception ex)
            {
                // Can't connect to server. Try again after waiting for a few minutes.
                DisplayErrorMessage("Can't update Weather information", ex.Message);
                if (weatherTimer.IsEnabled) weatherTimer.Stop();

                // Try to refresh data. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 10000);
                RefreshWeatherModel(null, null);
            }
        }

        private void FillWeatherInformation(WeatherModel weatherModel)
        {
            Temperature = weatherModel.Temperature;
            WeatherImage = weatherModel.Icon;
            WeatherType = weatherModel.WeatherType;
            Sunrise = weatherModel.SunRise;
            Sunset = weatherModel.SunSet;
            Location = weatherModel.Location;
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
                var settings = App.UserSettings;
                TrafficModel trafficModel = await _trafficService.GetModelAsync(settings.HomeAddress, settings.HomeCity, settings.WorkAddress);
                TravelDuration = trafficModel.Summary;

                if (!trafficTimer.IsEnabled) trafficTimer.Start();
            }
            catch (HttpRequestException)
            {
                TrafficModel trafficModel = _trafficService.GetOfflineModel(localFolder);
                TravelDuration = trafficModel.Summary;

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

        #endregion Methods

        #region Properties

        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                NotifyPropertyChanged();
            }
        }

        public string WeatherImage
        {
            get { return weatherimage; }
            set
            {
                weatherimage = value;
                NotifyPropertyChanged();
            }
        }

        public string WeatherType
        {
            get { return weatherType; }
            set
            {
                weatherType = value;
                NotifyPropertyChanged();
            }
        }

        public string Sunrise
        {
            get { return sunrise; }
            set
            {
                sunrise = value;
                NotifyPropertyChanged();
            }
        }

        public string Sunset
        {
            get { return sunset; }
            set
            {
                sunset = value;
                NotifyPropertyChanged();
            }
        }

        public string Compliment
        {
            get => compliment;
            set
            {
                compliment = value;
                NotifyPropertyChanged();
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                NotifyPropertyChanged();
            }
        }

        public string TravelDuration
        {
            get { return travelDuration; }
            set
            {
                travelDuration = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged();
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged();
            }
        }

        public RSSModel RSSInfo
        {
            get => _rss;
            set
            {
                _rss = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Properties
    }
}