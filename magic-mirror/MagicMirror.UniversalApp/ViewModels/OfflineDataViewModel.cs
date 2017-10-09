using System;
using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.Views;
using MagicMirror.Business.Services;
using Windows.Storage;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class OfflineDataViewModel : ViewModelBase
    {
        private WeatherService _weatherService;
        private TrafficService _trafficService;

        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public OfflineDataViewModel()
        {
            _navigationService.Navigate(typeof(MainPage));
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();
        }

        public void NavigateToMain()
        {
            try
            {
                _navigationService.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Main Page", ex.Message);
            }
        }

        public void SaveData(OfflineDataViewModel viewModel)
        {
            try
            {
                _weatherService.SaveOfflineModel(viewModel.WeatherModel, localFolder.Path);
            }
            catch(Exception ex)
            {
                DisplayErrorMessage("Unable to save offline Weather data", ex.Message);
            }

            try
            {
                _weatherService.SaveOfflineModel(viewModel.WeatherModel, localFolder.Path);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to save offline Traffic data", ex.Message);
            }
        }

        #region Properties
        private WeatherModel _weatherModel;
        public WeatherModel WeatherModel
        {
            get
            {
                return _weatherService.GetOfflineModel(localFolder.Path);
            }
            set
            {
                _weatherModel = value;
                OnPropertyChanged();
            }
        }

        private TrafficModel _trafficModel;
        public TrafficModel TrafficModel
        {
            get
            {
                return _trafficService.GetOfflineModel(localFolder.Path);
            }
            set
            {
                _trafficModel = value;
                OnPropertyChanged();
            }
        } 

        #endregion
    }
}
