using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class OfflineDataViewModel : ViewModelBase
    {
        private WeatherService _weatherService;
        private TrafficService _trafficService;

        public OfflineDataViewModel()
        {
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();

            _weatherModel = _weatherService.GetOfflineModel(localFolder);
            _trafficModel = _trafficService.GetOfflineModel(localFolder);
        }

        public void SaveData(OfflineDataViewModel viewModel)
        {
            try
            {
                _weatherService.SaveOfflineModel(viewModel.WeatherModel, localFolder);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to save offline Weather data", ex.Message);
            }

            try
            {
                _weatherService.SaveOfflineModel(viewModel.WeatherModel, localFolder);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to save offline Traffic data", ex.Message);
            }

            NavigateToMain();
        }

        #region Properties

        private WeatherModel _weatherModel;

        public WeatherModel WeatherModel
        {
            get
            {
                return _weatherModel;
            }
            set
            {
                _weatherModel = value;
                NotifyPropertyChanged();
            }
        }

        private TrafficModel _trafficModel;

        public TrafficModel TrafficModel
        {
            get
            {
                return _trafficModel;
            }
            set
            {
                _trafficModel = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Properties
    }
}