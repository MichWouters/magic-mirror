using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Services from the Business Layer
        private IApiService<WeatherModel> _weatherService;

        private IApiService<TrafficModel> _trafficService;

        public void NavigateToSettings()
        {
            try
            {
                _navigationService.Navigate(typeof(SettingPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Settins", ex.Message);
            }
        }

        private CommonService _commonService;

        // Timers to refresh individual components
        private DispatcherTimer timeTimer;
        private DispatcherTimer complimentTimer;
        private DispatcherTimer weatherTimer;
        private DispatcherTimer trafficTimer;

        public MainPageViewModel()
        {
            SetUpServices();
            SetUpTimers();
            LoadDataOnPageStartup();
            SetRefreshTimers();
        }

        private void SetUpServices()
        {
            try
            {
                App appReference = Application.Current as App;
                SearchCriteria searchCriteria = appReference.Criteria;

                _commonService = new CommonService();
                _weatherService = new WeatherService(searchCriteria);
                _trafficService = new TrafficService(searchCriteria);
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
                trafficTimer = new DispatcherTimer();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to initialize one or more timers.", ex.Message);
            }
        }

        /// <summary>
        /// Call data immediately after app launch
        /// </summary>
        private void LoadDataOnPageStartup()
        {
            RefreshTime(null, null);
            RefreshCompliment(null, null);
            RefreshWeatherModel(null, null);
            RefreshTrafficModel(null, null);
        }

        /// <summary>
        /// Set timers at which data needs to be refreshed
        /// </summary>
        private void SetRefreshTimers()
        {
            SetUpTimer(timeTimer, new TimeSpan(0, 0, 1), RefreshTime);
            SetUpTimer(complimentTimer, new TimeSpan(0, 5, 0), RefreshCompliment);
            SetUpTimer(weatherTimer, new TimeSpan(0, 15, 0), RefreshWeatherModel);
            SetUpTimer(trafficTimer, new TimeSpan(0, 10, 0), RefreshTrafficModel);

            //TODO: Write methods, then shorten them using new method
            //complimentTimer.Tick += RefreshCompliment;
            //complimentTimer.Interval = new TimeSpan(0, 1, 0);
            //if (!complimentTimer.IsEnabled) complimentTimer.Start();
        }

        private void SetUpTimer(DispatcherTimer timer, TimeSpan timeSpan, EventHandler<object> method)
        {
            timeTimer.Tick += method;
            timeTimer.Interval = new TimeSpan(0, 0, 10);
            if (!timeTimer.IsEnabled) timeTimer.Start();
        }

        private void RefreshTime(object sender, object e)
        {
            try
            {
                Time = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Cannot set Time", ex.Message);
            }
        }

        private void RefreshCompliment(object sender, object e)
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
                WeatherModel weatherModel = await _weatherService.GetModelAsync();
                WeatherInfo = weatherModel;

                if (!weatherTimer.IsEnabled) weatherTimer.Start();
            }
            catch (Exception ex)
            {
                // Can't connect to server. Try again after waiting for a few minutes
                DisplayErrorMessage("Can't update Weather information", ex.Message);
                if (weatherTimer.IsEnabled) weatherTimer.Stop();

                // Try to refresh data. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 1000);
                RefreshWeatherModel(null, null);
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
            catch (Exception ex)
            {
                // Can't connect to server. Try again after waiting for a few minutes
                DisplayErrorMessage("Can't update Traffic information", ex.Message);
                if (weatherTimer.IsEnabled) weatherTimer.Stop();

                // Try to refresh data immediately. If succesful, resume timer
                int minutes = 5;
                await Task.Delay((minutes * 60) * 1000);
                RefreshTrafficModel(null, null);
            }
        }

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

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Properties
    }
}